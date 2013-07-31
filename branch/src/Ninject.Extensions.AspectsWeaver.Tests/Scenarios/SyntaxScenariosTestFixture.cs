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
        private FakeInterceptor fakeInterceptor;

        [TestInitialize]
        public void Initialize()
        {
            kernel = new StandardKernel();

            kernel.Bind<FakeInterceptor>().ToSelf().InSingletonScope();

            this.fooBindingSyntax = kernel.Bind<IFoo>().To<Foo>()
                         .InSingletonScope();

            this.multiFooBindingSyntax = kernel.Bind<IMultiFoo>().To<MultiFoo>()
                         .InSingletonScope();
           
            fakeInterceptor = this.kernel.Get<FakeInterceptor>();
        }

        [TestMethod]
        public void WeaveTwiceWithTwoWeavers()
        {
            fooBindingSyntax.Weave().Into<FakeInterceptor>();
            fooBindingSyntax.Weave().Into<FakeInterceptor>();

            var foo = kernel.Get<IFoo>();
            foo.FooMe();

            fakeInterceptor = this.kernel.Get<FakeInterceptor>();
            Assert.AreEqual(2, fakeInterceptor.CalledTimes);
        }

        [TestMethod]
        public void WeaveIntoTwiceOnSameWeaver()
        {
            var syntax = fooBindingSyntax.Weave();

            syntax.Into<FakeInterceptor>();
            syntax.Into<FakeInterceptor>();

            var foo = kernel.Get<IFoo>();
            foo.FooMe();

            Assert.AreEqual(2, fakeInterceptor.CalledTimes);
        }

        [TestMethod]
        public void OnceJointPoints()
        {
            var syntax = multiFooBindingSyntax.Weave();

            syntax.JointPoints(new Foo23JoinPointSelector()).Into<FakeInterceptor>();

            var foo = kernel.Get<IMultiFoo>();
            foo.Foo1();
            foo.Foo2();
            foo.Foo3();

            Assert.AreEqual(2, fakeInterceptor.CalledTimes);
        }

        [TestMethod]
        public void TwiceJointPoints_SameSelector()
        {
            var syntax = multiFooBindingSyntax.Weave();

            syntax.JointPoints(new Foo23JoinPointSelector()).Into<FakeInterceptor>();
            syntax.JointPoints(new Foo23JoinPointSelector()).Into<FakeInterceptor>();

            var foo = kernel.Get<IMultiFoo>();
            foo.Foo1();
            foo.Foo2();
            foo.Foo3();

            Assert.AreEqual(4, fakeInterceptor.CalledTimes);
            Assert.AreEqual(0, fakeInterceptor.CalledMethods.Count(c => c == "Foo1"));
            Assert.AreEqual(0, fakeInterceptor.CalledMethods.Count(c => c == "Foo1"));
        }

        [TestMethod]
        public void TwiceJointPoints_DifferentSelectors_JointPointHasNotBeenRebinded()
        {
            var syntax = multiFooBindingSyntax.Weave();

            syntax.JointPoints(new Foo1JoinPointSelector()).Into<FakeInterceptor>();
            syntax.JointPoints(new Foo2JoinPointSelector()).Into<FakeInterceptor>();

            var foo = kernel.Get<IMultiFoo>();
            foo.Foo1();
            foo.Foo2();
            foo.Foo3();

            Assert.AreEqual(2, fakeInterceptor.CalledTimes);
            Assert.AreEqual(1, fakeInterceptor.CalledMethods.Count(c => c == "Foo1"));
            Assert.AreEqual(1, fakeInterceptor.CalledMethods.Count(c => c == "Foo2"));
            Assert.AreEqual(0, fakeInterceptor.CalledMethods.Count(c => c == "Foo3"));
        }

        public interface IMultiFoo
        {
            void Foo1();
            void Foo2();
            void Foo3();
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
        }

        public class Foo1JoinPointSelector : IJointPointSelector
        {
            public bool IsJointPoint(Type type, MethodInfo method)
            {
                return method.Name == "Foo1";
            }
        }

        public class Foo2JoinPointSelector : IJointPointSelector
        {
            public bool IsJointPoint(Type type, MethodInfo method)
            {
                return method.Name == "Foo2";
            }
        }

        public class Foo23JoinPointSelector : IJointPointSelector
        {
            public bool IsJointPoint(Type type, MethodInfo method)
            {
                return method.Name == "Foo2" || method.Name == "Foo3";
            }
        }
    }
}