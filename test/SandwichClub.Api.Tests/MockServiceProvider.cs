using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Moq;

namespace SandwichClub.Api.Tests
{
    public class MockServiceProvider : IServiceProvider
    {
        private readonly ConcurrentDictionary<Type, object> _services;
        private readonly ISet<Type> _mockedServices;

        public MockServiceProvider()
        {
            _services = new ConcurrentDictionary<Type, object>();
            _mockedServices = new HashSet<Type>();
        }

        public object GetService(Type serviceType)
        {
            var mockType = typeof(Mock<>).MakeGenericType(serviceType);

            dynamic instance = _services.GetOrAdd(serviceType, k =>
            {
                _mockedServices.Add(serviceType);
                return mockType
                    .GetConstructor(new [] {typeof(MockBehavior)})
                    .Invoke(new object[] { MockBehavior.Strict });
            });

            return instance.GetType() == mockType ? instance.Object : instance;
        }

        public Mock<T> GetMock<T>() where T : class
        {
            return (Mock<T>) _services.GetOrAdd(typeof(T), k =>
            {
                _mockedServices.Add(k);
                return Activator.CreateInstance(typeof(Mock<>).MakeGenericType(k));
            });
        }

        public void RegisterInstance<T>(T instance)
        {
            RegisterInstance<T>(instance, typeof(T));
        }

        public void RegisterInstance<T>(T instance, Type asType)
        {
            _services[asType] = instance;
            _mockedServices.Remove(asType);
        }

        public bool IsRegistered(Type type)
        {
            return _services.ContainsKey(type);
        }
    }
}
