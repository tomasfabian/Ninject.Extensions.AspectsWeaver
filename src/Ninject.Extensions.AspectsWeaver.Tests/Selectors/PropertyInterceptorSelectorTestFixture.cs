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
using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Extensions.AspectsWeaver.Attributes;
using Ninject.Extensions.AspectsWeaver.Planning.Bindings;
using Ninject.Extensions.AspectsWeaver.Selectors;
using Ninject.Extensions.AspectsWeaver.Tests.Fakes;

namespace Ninject.Extensions.AspectsWeaver.Tests.Selectors
{
    [TestClass]
    public class PropertyInterceptorSelectorTestFixture
    {
        private IKernel kernel;
        private IFooWithGetter foo;
        private IPointCutSelector cutSelector;

        [TestInitialize]
        public void Initialize()
        {
            this.cutSelector = new GetPropertyPointCutSelector<IFooWithGetter>(f => f.Foo);
            kernel = new StandardKernel();

            kernel.Bind<FakeAspect>().ToSelf().InSingletonScope();

            kernel.Bind<IFooWithGetter>().To<FooWithGetter>()
                         .InSingletonScope()
                         .Weave()
                         .PointCuts(cutSelector)
                         .Into<FakeAspect>();

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
            var interceptor = this.kernel.Get<FakeAspect>();
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
            var interceptor = this.kernel.Get<FakeAspect>();
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

        [ExcludeJointPoint]
        public int NotFoo
        {
            get { return -1; }
        }
    }
}