// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject.Extensions.AspectsWeaver.Planning.Bindings;
using Ninject.Extensions.AspectsWeaver.Tests.Fakes;
using Ninject.Extensions.AspectsWeaver.Tests.Selectors;
using Ninject.Syntax;

namespace Ninject.Extensions.AspectsWeaver.Tests.Scenarios
{
    [TestClass]
    public class MemberTypesInterceptionTestFixture
    {
        private IFooWithEverything<string> foo;
        private IKernel kernel;
        private IBindingNamedWithOrOnSyntax<FooWithEverything<string>> fooBindingSyntax;
        private IBindingNamedWithOrOnSyntax<SyntaxScenariosTestFixture.MultiFoo> multiFooBindingSyntax;
        private FakeAspect fakeAspect;
        private FakeAspect2 fakeAspect2;

        [TestInitialize]
        public void Initialize()
        {
            kernel = new StandardKernel();

            kernel.Bind<FakeAspect>().ToSelf().InSingletonScope();
            kernel.Bind<FakeAspect2>().ToSelf().InSingletonScope();

            this.fooBindingSyntax = kernel.Bind<IFooWithEverything<string>>().To<FooWithEverything<string>>()
                         .InSingletonScope();

            fakeAspect = this.kernel.Get<FakeAspect>();
            fakeAspect2 = this.kernel.Get<FakeAspect2>();

            fooBindingSyntax.Weave().Into<FakeAspect>();

            foo = kernel.Get<IFooWithEverything<string>>();
        }

        [TestMethod]
        public void Property()
        {
            var result = foo.Property;

            Assert.AreEqual(1, fakeAspect.CalledTimes);
        }

        [TestMethod]
        public void PropertyGetter()
        {
            var result = foo.PropertyGetter;

            Assert.AreEqual(1, fakeAspect.CalledTimes);
        }

        [TestMethod]
        public void PropertySetter()
        {
            foo.PropertySetter = true;

            Assert.AreEqual(1, fakeAspect.CalledTimes);
        }

        [TestMethod]
        public void VoidMethod()
        {
            foo.VoidMethod();

            Assert.AreEqual(1, fakeAspect.CalledTimes);
        }
        
        [TestMethod]
        public void MethodWithReturnValue()
        {
            foo.MethodWithReturnValue();

            Assert.AreEqual(1, fakeAspect.CalledTimes);
        }
        
        [TestMethod]
        public void VoidMethodWithArgument()
        {
            foo.VoidMethodWithArgument("a");

            Assert.AreEqual(1, fakeAspect.CalledTimes);
        }


        [TestMethod]
        public void VoidMethodWithArgument_SameNameDifferentSignature()
        {
            foo.VoidMethodWithArgument(1);

            Assert.AreEqual(1, fakeAspect.CalledTimes);
        }

        [TestMethod]
        public void GenericVoidMethod()
        {
            foo.GenericVoidMethod();

            Assert.AreEqual(1, fakeAspect.CalledTimes);
        }

        [TestMethod]
        public void GenericVoidMethodWithArgument()
        {
            foo.GenericVoidMethodWithArgument("a");

            Assert.AreEqual(1, fakeAspect.CalledTimes);
        }

        [TestMethod]
        public void GenericVoidMethodWithArgument_SameSignature()
        {
            foo.GenericVoidMethodWithArgument("a");

            Assert.AreEqual(1, fakeAspect.CalledTimes);
        }
    }


    public interface IFooWithEverything<T>
    {
        bool Property { get; set; }
        bool PropertyGetter { get; }
        bool PropertySetter { set; }
        void VoidMethod();
        bool MethodWithReturnValue();
        void VoidMethodWithArgument(string arg1);
        void VoidMethodWithArgument(int arg1);
        void VoidMethodWithArgument2(string arg2);
        void GenericVoidMethod();
        void GenericVoidMethodWithArgument(T arg1);
    }

    public class FooWithEverything<T> : IFooWithEverything<T>
    {
        public bool Property { get; set; }

        private bool propertyGetter;
        public bool PropertyGetter { get { return this.propertyGetter; } }

        private bool propertySetter;
        public bool PropertySetter { set { this.propertySetter = value;  } }

        public void VoidMethod()
        {
        }

        public bool MethodWithReturnValue()
        {
            return true;
        }

        public void VoidMethodWithArgument(string arg1)
        {
        }

        public void VoidMethodWithArgument(int arg1)
        {
        }

        public void VoidMethodWithArgument2(string arg1)
        {
        }

        public void GenericVoidMethod()
        {
        }

        public void GenericVoidMethodWithArgument(T arg1)
        {
        }

        public FooWithEverything()
        {
        }



    }
}