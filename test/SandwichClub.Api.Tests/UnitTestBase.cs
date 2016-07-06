using System.Linq;
using System.Reflection;
using Moq;

namespace SandwichClub.Api.Tests
{
    public abstract class UnitTestBase<T> where T : class
    {
        protected readonly MockServiceProvider ServiceProvider;

        public T Service { get; }

        protected UnitTestBase()
        {
            ServiceProvider = new MockServiceProvider();

            Service = CreateService();
        }

        private T CreateService()
        {
            var constructor = typeof(T).GetConstructors().First(c => c.IsPublic);
            var constructorParams = constructor.GetParameters();

            var mocks = new object[constructorParams.Length];

            for (var i = 0; i < constructorParams.Length; ++i)
            {
                mocks[i] = ServiceProvider.GetService(constructorParams[i].ParameterType);
            }

            return (T) constructor.Invoke(mocks);
        }

        protected Mock<TService> Mock<TService>() where TService : class
        {
            return ServiceProvider.GetMock<TService>();
        }
    }
}
