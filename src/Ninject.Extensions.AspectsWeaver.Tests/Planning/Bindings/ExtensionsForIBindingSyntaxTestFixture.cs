// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject.Extensions.AspectsWeaver.Planning.Bindings;
using Ninject.Extensions.AspectsWeaver.Tests.Fakes;
using Ninject.Extensions.AspectsWeaver.Tests.Selectors;

namespace Ninject.Extensions.AspectsWeaver.Tests.Planning.Bindings
{
    [TestClass]
    public class ExtensionsForIBindingSyntaxTestFixture
    {
        private IKernel kernel;
        private IFooWithGetter foo;


        [TestInitialize]
        public void Initialize()
        {
            kernel = new StandardKernel();

            kernel.Bind<FakeAspect>().ToSelf().InSingletonScope();

            kernel.Bind<IFooWithGetter>().To<FooWithGetter>()
                         .WeaveGetProperty(f => f.Foo)
                         .Into<FakeAspect>();

            foo = kernel.Get<IFooWithGetter>();
        }

        [TestCleanup]
        public void CleanUp()
        {
            this.kernel.Dispose();
        }

        [TestMethod]
        public void InterceptProperty_AllowedPropertyGetterHasBeenIntercepted()
        {
            //Arrange
            var interceptor = this.kernel.Get<FakeAspect>();
            interceptor.GetReturnValue = () => 1;

            //Act
            int result = foo.Foo;

            //Assert
            Assert.AreEqual(1, interceptor.CalledTimes);
            CollectionAssert.Contains(interceptor.CalledMethods.ToList(), "get_Foo");
        }

        [TestMethod]
        public void InterceptProperty_NotAllowedPropertyGetterHasNotBeenIntercepted()
        {
            //Arrange
            var interceptor = this.kernel.Get<FakeAspect>();
            interceptor.GetReturnValue = () => 1;

            //Act
            int result = foo.NotFoo;

            //Assert
            Assert.AreEqual(0, interceptor.CalledTimes);
            CollectionAssert.DoesNotContain(interceptor.CalledMethods.ToList(), "get_NotFoo");
        }

        [TestMethod]
        public void InterceptExcludedProperty_NotAllowedPropertyGetterHasNotBeenIntercepted()
        {
            //Arrange
            kernel.Rebind<IFooWithGetter>().To<FooWithGetter>()
             .WeaveGetProperty(f => f.NotFoo)
             .Into<FakeAspect>();

            foo = kernel.Get<IFooWithGetter>();

            var interceptor = this.kernel.Get<FakeAspect>();
            interceptor.GetReturnValue = () => 1;

            //Act
            int result = foo.NotFoo;

            //Assert
            Assert.AreEqual(0, interceptor.CalledTimes);
            CollectionAssert.DoesNotContain(interceptor.CalledMethods.ToList(), "get_NotFoo");
        }
    }
}