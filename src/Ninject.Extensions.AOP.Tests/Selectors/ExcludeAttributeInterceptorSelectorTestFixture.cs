// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
using System;
using System.Linq;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject.Extensions.AOP.Attributes;
using Ninject.Extensions.AOP.Planning.Bindings;
using Ninject.Extensions.AOP.Tests.Fakes;

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

            kernel.Bind<FakeInterceptor>().ToSelf().InSingletonScope();

            kernel.Bind<IFoo>().To<Foo>()
                         .InSingletonScope()
                         .Intercept()
                         .With<FakeInterceptor>();

            foo = kernel.Get<IFoo>();
        }

        [TestCleanup]
        public void CleanUp()
        {
            this.kernel.Dispose();
        }

        [TestMethod]
        public void NotExcludedMethodHasBeenIntercepted()
        {
            //Arrange
            var interceptor = this.kernel.Get<FakeInterceptor>();

            //Act
            foo.FooMe();
                        
            //Assert
            Assert.AreEqual(1, interceptor.CalledTimes);
            CollectionAssert.Contains(interceptor.CalledMethods.ToList(), "FooMe");
        }

        [TestMethod]
        public void ExcludedMethodHasNotBeenIntercepted()
        {
            //Arrange
            var interceptor = this.kernel.Get<FakeInterceptor>();

            //Act
            foo.Dispose();

            //Assert
            Assert.AreEqual(0, interceptor.CalledTimes);
            CollectionAssert.DoesNotContain(interceptor.CalledMethods.ToList(), "Dispose");
        }

        [TestMethod]
        public void ClassLevelExclude_NothingHasBeenIntercepted()
        {
            //Arrange
            kernel.Rebind<IFoo>().To<FooWithClassLevelExlude>()
             .InSingletonScope()
             .Intercept()
             .With<FakeInterceptor>();

            foo = kernel.Get<IFoo>();
            var interceptor = this.kernel.Get<FakeInterceptor>();

            //Act
            foo.FooMe();
            foo.Dispose();

            //Assert
            Assert.AreEqual(0, interceptor.CalledTimes);
            Assert.IsFalse(interceptor.CalledMethods.Any());
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
        }

        [ExcludeFromInterception]
        public void Dispose()
        {
        }
    }
        
    [ExcludeFromInterception]
    public class FooWithClassLevelExlude : IFoo
    {
        public void FooMe()
        {
        }

        public void Dispose()
        {
        }
    }
}