// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System.Linq;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject.Extensions.AOP.Planning.Bindings;
using Ninject.Extensions.AOP.Selectors;
using Ninject.Extensions.AOP.Tests.Fakes;

namespace Ninject.Extensions.AOP.Tests.Selectors
{
    [TestClass]
    public class PropertyInterceptorSelectorTestFixture
    {
        private IKernel kernel;
        private IFooWithGetter foo;
        private IAllowInterceptionSelector interceptorSelector;

        [TestInitialize]
        public void Initialize()
        {
            this.interceptorSelector = new PropertyInterceptorSelector<IFooWithGetter>(f => f.Foo);
            kernel = new StandardKernel();

            kernel.Bind<FakeInterceptor>().ToSelf().InSingletonScope();

            kernel.Bind<IFooWithGetter>().To<FooWithGetter>()
                         .InSingletonScope()
                         .Intercept()
                         .With<FakeInterceptor>()
                         .AllowInterceptionWith(interceptorSelector);

            foo = kernel.Get<IFooWithGetter>();
        }

        [TestCleanup]
        public void CleanUp()
        {
            this.kernel.Dispose();
        }

        [TestMethod]
        public void AllowedPropertyGetterHasBeenIntercepted()
        {
            //Arrange
            var interceptor = this.kernel.Get<FakeInterceptor>();
            interceptor.GetReturnValue = () => 1;

            //Act
            int result = foo.Foo;

            //Assert
            Assert.AreEqual(1, interceptor.CalledTimes);
            CollectionAssert.Contains(interceptor.CalledMethods.ToList(), "get_Foo");
        }

        [TestMethod]
        public void NotAllowedPropertyGetterHasNotBeenIntercepted()
        {
            //Arrange
            var interceptor = this.kernel.Get<FakeInterceptor>();
            interceptor.GetReturnValue = () => 1;

            //Act
            int result = foo.NotFoo;

            //Assert
            Assert.AreEqual(0, interceptor.CalledTimes);
            CollectionAssert.DoesNotContain(interceptor.CalledMethods.ToList(), "get_NotFoo");
        }
    }

    public interface IFooWithGetter 
    {
        int Foo { get; }
        int NotFoo { get; }
    }

    public class FooWithGetter : IFooWithGetter
    {
        public int Foo
        {
            get { return 1; }
        }

        public int NotFoo
        {
            get { return -1; }
        }
    }
}