using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject.Extensions.AOP.Attributes;
using Ninject.Extensions.AOP.Planning.Bindings;

namespace Ninject.Extensions.AOP.Tests.Selectors
{
    [TestClass]
    public class ExcludeAttributeInterceptorSelectorTestFixture
    {
        private IKernel kernel;
        private IFoo foo;

        [TestInitialize]
        public void Initialize()
        {
            kernel = new StandardKernel();

            kernel.Bind<IFoo>().To<Foo>()
                         .InSingletonScope()
                         .Intercept()
                         .With<LoggingInterceptor>()
                         .With<LoggingInterceptor>();

            foo = kernel.Get<IFoo>();

        }

        [TestCleanup]
        public void CleanUp()
        {
            this.kernel.Dispose();
        }

        [TestMethod]
        public void test()
        {
            //Act
            foo.FooMe();
                        
            //Arrange
        }
    }

    public interface IFoo : IDisposable
    {
        void FooMe();
    }

    public class Foo : IFoo
    {
        public void FooMe()
        {
            Console.WriteLine("FooMe");
        }

        [ExcludeFromInterception]
        public void Dispose()
        {
            Console.WriteLine("Foo disposed");
        }
    }

    public class LoggingInterceptor : Castle.DynamicProxy.IInterceptor
    {
        public void Intercept(Castle.DynamicProxy.IInvocation invocation)
        {
            Console.WriteLine("Log Before");

            invocation.Proceed();

            Console.WriteLine("Log After");
        }
    }

}