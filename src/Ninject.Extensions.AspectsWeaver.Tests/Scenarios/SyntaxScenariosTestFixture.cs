// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Extensions.AspectsWeaver.Attributes;
using Ninject.Extensions.AspectsWeaver.Planning.Bindings;
using Ninject.Extensions.AspectsWeaver.Tests.Fakes;
using Ninject.Extensions.AspectsWeaver.Tests.Selectors;
using Ninject.Syntax;

namespace Ninject.Extensions.AspectsWeaver.Tests.Scenarios
{
    [TestClass]
    public class SyntaxScenariosTestFixture
    {
        private IKernel kernel;
        private IBindingNamedWithOrOnSyntax<Foo> fooBindingSyntax;
        private IBindingNamedWithOrOnSyntax<MultiFoo> multiFooBindingSyntax;
        private FakeAspect fakeAspect;
        private FakeAspect2 fakeAspect2;

        [TestInitialize]
        public void Initialize()
        {
            kernel = new StandardKernel();

            kernel.Bind<FakeAspect>().ToSelf().InSingletonScope();
            kernel.Bind<FakeAspect2>().ToSelf().InSingletonScope();

            this.fooBindingSyntax = kernel.Bind<IFoo>().To<Foo>()
                         .InSingletonScope();

            this.multiFooBindingSyntax = kernel.Bind<IMultiFoo>().To<MultiFoo>()
                         .InSingletonScope();
           
            fakeAspect = this.kernel.Get<FakeAspect>();
            fakeAspect2 = this.kernel.Get<FakeAspect2>();
        }

        [TestMethod]
        public void Empty()
        {
            fooBindingSyntax.Weave();

            var foo = kernel.Get<IFoo>();
            foo.FooMe();

            Assert.IsNotNull(foo);
        }

        [TestMethod]
        public void OneTimeWeaving()
        {
            fooBindingSyntax.Weave().Into<FakeAspect>();

            var foo = kernel.Get<IFoo>();
            foo.FooMe();

            Assert.AreEqual(1, fakeAspect.CalledTimes);
        }

        [TestMethod]
        public void EmptyAndOneTimeWeaving()
        {
            fooBindingSyntax.Weave();
            fooBindingSyntax.Weave().Into<FakeAspect>();

            var foo = kernel.Get<IFoo>();
            foo.FooMe();

            Assert.AreEqual(1, fakeAspect.CalledTimes);
        }

        [TestMethod]
        public void OneTimeWeavingAndEmpty()
        {
            fooBindingSyntax.Weave().Into<FakeAspect>();
            fooBindingSyntax.Weave();

            var foo = kernel.Get<IFoo>();
            foo.FooMe();

            Assert.AreEqual(1, fakeAspect.CalledTimes);
        }

        [TestMethod]
        public void WeavedIntoTwoAspects()
        {
            fooBindingSyntax.Weave()
                .Into<FakeAspect>()
                .Into<FakeAspect2>();

            var foo = kernel.Get<IFoo>();
            foo.FooMe();

            Assert.AreEqual(1, fakeAspect.CalledTimes);
            Assert.AreEqual(1, fakeAspect2.CalledTimes);
        }

        [TestMethod]
        public void WeavedInToBranches()
        {
            fooBindingSyntax.Weave().Into<FakeAspect>();
            fooBindingSyntax.Weave().Into<FakeAspect2>();

            var foo = kernel.Get<IFoo>();
            foo.FooMe();

            Assert.AreEqual(1, fakeAspect.CalledTimes);
            Assert.AreEqual(1, fakeAspect2.CalledTimes);
        }

        [TestMethod]
        public void Split_WeavedIntoTwiceOnSameWeaver()
        {
            var syntax = fooBindingSyntax.Weave();

            syntax.Into<FakeAspect>();
            syntax.Into<FakeAspect>();

            var foo = kernel.Get<IFoo>();
            foo.FooMe();

            Assert.AreEqual(2, fakeAspect.CalledTimes);
        }

        [TestMethod]
        public void Split_WeavedInTwoBranches_SecondIsWeavedIntoTwoAspects()
        {
            var syntax = fooBindingSyntax.Weave();

            syntax.Into<FakeAspect>();
            syntax.Into<FakeAspect>()
                .Into<FakeAspect2>();

            var foo = kernel.Get<IFoo>();
            foo.FooMe();

            Assert.AreEqual(2, fakeAspect.CalledTimes);
            Assert.AreEqual(1, fakeAspect2.CalledTimes);
        }

        [TestMethod]
        public void EmptyJointPoints()
        {
            var syntax = multiFooBindingSyntax.Weave();

            syntax.PointCuts(new Foo23JoinPointCutSelector());

            var foo = kernel.Get<IMultiFoo>();
            this.CallFooMethods(foo);

            Assert.AreEqual(0, fakeAspect.CalledTimes);
        }

        [TestMethod]
        public void OnceJointPoints()
        {
            var syntax = multiFooBindingSyntax.Weave();

            syntax.PointCuts(new Foo23JoinPointCutSelector())
                .Into<FakeAspect>();

            var foo = kernel.Get<IMultiFoo>();
            this.CallFooMethods(foo);

            Assert.AreEqual(2, fakeAspect.CalledTimes);
        }

        [TestMethod]
        public void OnePointCutForTwoAspects()
        {
            var syntax = multiFooBindingSyntax.Weave();

            syntax.PointCuts(new Foo23JoinPointCutSelector())
                .Into<FakeAspect>()
                .Into<FakeAspect2>();

            var foo = kernel.Get<IMultiFoo>();
            this.CallFooMethods(foo);

            Assert.AreEqual(2, fakeAspect.CalledTimes);
            Assert.AreEqual(2, fakeAspect2.CalledTimes);
        }

        [TestMethod]
        public void TwiceJointPoints_SameSelector()
        {
            var syntax = multiFooBindingSyntax.Weave();

            syntax.PointCuts(new Foo23JoinPointCutSelector()).Into<FakeAspect>();
            syntax.PointCuts(new Foo23JoinPointCutSelector()).Into<FakeAspect>();

            var foo = kernel.Get<IMultiFoo>();
            this.CallFooMethods(foo);

            Assert.AreEqual(4, fakeAspect.CalledTimes);
            Assert.AreEqual(0, fakeAspect.CalledMethods.Count(c => c == "Foo1"));
            Assert.AreEqual(0, fakeAspect.CalledMethods.Count(c => c == "Foo1"));
        }

        [TestMethod]
        public void TwiceJointPoints_DifferentSelectors_JointPointHasNotBeenRebinded()
        {
            var syntax = multiFooBindingSyntax.Weave();

            syntax.PointCuts(new Foo1JoinPointCutSelector()).Into<FakeAspect>();
            syntax.PointCuts(new Foo2JoinPointCutSelector()).Into<FakeAspect>();

            var foo = kernel.Get<IMultiFoo>();
            this.CallFooMethods(foo);

            Assert.AreEqual(2, fakeAspect.CalledTimes);
            Assert.AreEqual(1, fakeAspect.CalledMethods.Count(c => c == "Foo1"));
            Assert.AreEqual(1, fakeAspect.CalledMethods.Count(c => c == "Foo2"));
            Assert.AreEqual(0, fakeAspect.CalledMethods.Count(c => c == "Foo3"));
        }

        [TestMethod]
        public void TwiceJointPoints_OneBranchDoesNotHaveAspect()
        {
            var syntax = multiFooBindingSyntax.Weave();

            syntax.PointCuts(new Foo1JoinPointCutSelector()).Into<FakeAspect>();
            syntax.PointCuts(new Foo2JoinPointCutSelector());

            var foo = kernel.Get<IMultiFoo>();
            this.CallFooMethods(foo);

            Assert.AreEqual(1, fakeAspect.CalledTimes);
            Assert.AreEqual(1, fakeAspect.CalledMethods.Count(c => c == "Foo1"));
            Assert.AreEqual(0, fakeAspect.CalledMethods.Count(c => c == "Foo2"));
            Assert.AreEqual(0, fakeAspect.CalledMethods.Count(c => c == "Foo3"));
        }

        [TestMethod]
        public void TwiceJointPointsWithTwoInterceptors_DifferentSelectors_JointPointHasNotBeenRebinded()
        {
            var syntax = multiFooBindingSyntax.Weave();

            syntax.PointCuts(new Foo1JoinPointCutSelector()).Into<FakeAspect>();
            syntax.PointCuts(new Foo2JoinPointCutSelector())
                .Into<FakeAspect>()
                .Into<FakeAspect2>();

            var foo = kernel.Get<IMultiFoo>();
            this.CallFooMethods(foo);

            Assert.AreEqual(2, fakeAspect.CalledTimes);
            Assert.AreEqual(1, fakeAspect.CalledMethods.Count(c => c == "Foo1"));
            Assert.AreEqual(1, fakeAspect.CalledMethods.Count(c => c == "Foo2"));
            Assert.AreEqual(0, fakeAspect.CalledMethods.Count(c => c == "Foo3"));

            Assert.AreEqual(1, fakeAspect2.CalledTimes);
            Assert.AreEqual(0, fakeAspect2.CalledMethods.Count(c => c == "Foo1"));
            Assert.AreEqual(1, fakeAspect2.CalledMethods.Count(c => c == "Foo2"));
            Assert.AreEqual(0, fakeAspect2.CalledMethods.Count(c => c == "Foo3"));
        }

        private void CallFooMethods(IMultiFoo foo)
        {
            foo.Foo1();
            foo.Foo2();
            foo.Foo3();
            foo.Foo4();
        }

        public interface IMultiFoo
        {
            void Foo1();
            void Foo2();
            void Foo3();
            void Foo4();
        }

        public class MultiFoo : IMultiFoo
        {
            public void Foo1()
            {
            }

            public void Foo2()
            {
            }

            public void Foo3()
            {
            }

            [ExcludeJointPoint]
            public void Foo4()
            {
            }
        }

        public class Foo1JoinPointCutSelector : IPointCutSelector
        {
            public bool IsPointCut(Type type, MethodInfo method)
            {
                return method.Name == "Foo1";
            }
        }

        public class Foo2JoinPointCutSelector : IPointCutSelector
        {
            public bool IsPointCut(Type type, MethodInfo method)
            {
                return method.Name == "Foo2";
            }
        }

        public class Foo23JoinPointCutSelector : IPointCutSelector
        {
            public bool IsPointCut(Type type, MethodInfo method)
            {
                return method.Name == "Foo2" || method.Name == "Foo3";
            }
        }
    }
}