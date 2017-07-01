using AutoMapper;
using System;
using System.Linq;
using System.Reflection;
using SandwichClub.Api.Repositories;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SandwichClub.Api.Tests
{
    public abstract class UnitTestBase
    {
        static UnitTestBase()
        {
            Startup.InitializeAutoMapper();
        }

        protected readonly MockServiceProvider ServiceProvider;

        protected UnitTestBase()
        {
            ServiceProvider = new MockServiceProvider();

            ServiceProvider.RegisterInstance(Mapper.Instance, typeof(IMapper));
        }

        protected Mock<TService> Mock<TService>() where TService : class
        {
            return ServiceProvider.GetMock<TService>();
        }

        protected TRepository GetRepository<TRepository>() where TRepository : class
        {
            if (!ServiceProvider.IsRegistered(typeof(ScContext)))
            {
                // Create a fresh service provider, and therefore a fresh 
                // InMemory database instance.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Create a new options instance telling the context to use an
                // InMemory database and the new service provider.
                var builder = new DbContextOptionsBuilder<ScContext>();
                builder.UseInMemoryDatabase("test")
                    .UseInternalServiceProvider(serviceProvider);

                ServiceProvider.RegisterInstance(new ScContext(builder.Options));
            }

            return GetService<TRepository>(typeof(TRepository).GetInterfaces());
        }

        protected TService GetService<TService>() where TService : class
        {
            return GetService<TService>(typeof(TService));
        }

        protected TService GetService<TService>(params Type[] asTypes)
        {
            var constructor = typeof(TService).GetConstructors().First(c => c.IsPublic);
            var constructorParams = constructor.GetParameters();

            var dependencies = new object[constructorParams.Length];

            for (var i = 0; i < constructorParams.Length; ++i)
            {
                dependencies[i] = ServiceProvider.GetService(constructorParams[i].ParameterType);
            }

            var instance = (TService) constructor.Invoke(dependencies);
            foreach (var asType in asTypes)
            {
                ServiceProvider.RegisterInstance(instance, asType);
            }

            return instance;
        }
    }

    public abstract class UnitTestBase<T> : UnitTestBase where T : class
    {
        public T Service => LazyService.Value;
        private Lazy<T> LazyService { get; }

        protected UnitTestBase()
        {
            LazyService = new Lazy<T>(() => GetService<T>());

            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddConsole(LogLevel.Trace)
                .AddDebug(LogLevel.Trace);

            ServiceProvider.RegisterInstance(loggerFactory.CreateLogger<T>());
        }

        private T CreateService()
        {
            return GetService<T>(typeof(T).GetInterfaces());
        }
    }
}
