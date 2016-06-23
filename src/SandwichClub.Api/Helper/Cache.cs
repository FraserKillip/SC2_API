using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SandwichClub.Api.Helper
{
    public interface ICache<TKey, T>
    {
        Task<Cache<TKey, T>.CacheResult> TryGetItemAsync(TKey key);
        Task<T> GetOrAddAsync(TKey key, Func<TKey, Task<T>> add);
        Task<IEnumerable<T>> GetOrAddAsync(IEnumerable<TKey> keys, Func<TKey, Task<T>> addSingle);
        void AddItem(TKey key, T t);
        void InvalidateItem(TKey key);
        void Clear();
    }

    public class Cache<TKey, T> : ICache<TKey, T>
    {
        private CacheItems _items;

        private readonly int _cacheSize;

        public Cache() : this(int.MaxValue)
        {
        }

        public Cache(int cacheSize)
        {
            Init();
            _cacheSize = cacheSize;
        }

        protected void Init()
        {
            var items = _items;
            _items = new CacheItems();

            if (items != null)
            {
                // Assist garbage collection
                items.Keyed.Clear();
                items.Ordered.Clear();
            }
        }

        public async Task<CacheResult> TryGetItemAsync(TKey key)
        {
            var items = _items;

            CacheItem item;
            if (items.Keyed.TryGetValue(key, out item))
            {
                return new CacheResult(await item.Item.Value);
            }

            return new CacheResult();
        }

        public async Task<T> GetOrAddAsync(TKey key, Func<TKey, Task<T>> add)
        {
            var items = _items;
            var item = items.Keyed.GetOrAdd(key, GetAsyncCacheItem(key, add));

            return await item.Item.Value;
        }

        public async Task<IEnumerable<T>> GetOrAddAsync(IEnumerable<TKey> keys, Func<TKey, Task<T>> addSingle)
        {
            var items = await Task.WhenAll(keys.Select(key => GetOrAddAsync(key, addSingle)));

            if (typeof(T).IsByRef)
                return items.Where(i => i != null);
            return items;
        }

        public void AddItem(TKey key, T t)
        {
            var items = _items;

            var newItem = GetCacheItem(key, k => t);
            var item = items.Keyed.GetOrAdd(key, newItem);
            // Copy item
            item.Item = newItem.Item;

            AddItemToOrdered(items, item);
        }

        public void InvalidateItem(TKey key)
        {
            InvalidateItem(_items, key);
        }

        public void Clear()
        {
            Init();
        }

        private void AddItemToOrdered(CacheItems items, CacheItem item)
        {
            CacheItem removed = null;
            // Update order
            lock (items.Lock)
            {
                // Check if the node exists in the ordered list
                if (item.Node.List != null)
                {
                    // Remove it
                    items.Ordered.Remove(item.Node);
                }

                // Add new node
                items.Ordered.AddLast(item.Node);

                // Check size
                if (items.Ordered.Count > _cacheSize)
                {
                    removed = items.Ordered.First.Value;
                    if (removed != null)
                        items.Ordered.Remove(removed.Node);
                }
            }

            if (removed != null)
                InvalidateItem(items, removed.Key);
        }

        private void InvalidateItem(CacheItems items, TKey key)
        {
            CacheItem removed;
            items.Keyed.TryRemove(key, out removed);
            if (removed != null && removed.Node != null)
            {
                lock (items.Lock)
                {
                    items.Ordered.Remove(removed.Node);
                }
            }
        }

        private CacheItem GetCacheItem(TKey key, Func<TKey, T> addFunc)
        {
            return new CacheItem(key, new Lazy<Task<T>>(
                () => Task.FromResult(addFunc(key)),
                LazyThreadSafetyMode.ExecutionAndPublication));
        }

        private CacheItem GetAsyncCacheItem(TKey key, Func<TKey, Task<T>> addFunc)
        {
            return new CacheItem(key, new Lazy<Task<T>>(
                () => addFunc(key),
                LazyThreadSafetyMode.ExecutionAndPublication));
        }

        private class CacheItems
        {
            public readonly object Lock = new object();
            public readonly ConcurrentDictionary<TKey, CacheItem> Keyed = new ConcurrentDictionary<TKey, CacheItem>();
            public readonly LinkedList<CacheItem> Ordered = new LinkedList<CacheItem>();
        }

        private class CacheItem
        {
            public TKey Key { get; }
            public Lazy<Task<T>> Item { get; set; }
            public LinkedListNode<CacheItem> Node { get; }

            public CacheItem(TKey key, Lazy<Task<T>> item)
            {
                Key = key;
                Item = item;
                Node = new LinkedListNode<CacheItem>(this);
            }
        }

        public class CacheResult
        {
            public bool Cached { get; }
            public T Value { get; }

            public CacheResult()
            {
                Cached = false;
                Value = default(T);
            }

            public CacheResult(T value)
            {
                Cached = true;
                Value = value;
            }
        }
    }
}
