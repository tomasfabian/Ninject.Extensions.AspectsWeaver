// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;
using Ninject;
using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Extensions.AspectsWeaver.Modules;
using Ninject.Extensions.AspectsWeaver.Planning.Bindings;

namespace AspectWeaver.Demo.Basic
{
    internal class Program
    {
        private static void Main()
        {
            Ninject.IKernel kernel = new Ninject.StandardKernel(new AspectsWeaverModule());

            kernel.Bind<IFoo>().To<Foo>()
            .InSingletonScope()
            .Weave()
            .Into<LoggingAspect>();

            var foo = kernel.Get<IFoo>();
            foo.FooMe();
        }

        private void VirtualCase()
        {
            Ninject.IKernel kernel = new Ninject.StandardKernel(new AspectsWeaverModule());

            kernel.Bind<Foo>().ToSelf()
            .InSingletonScope()
            .Weave()
            .Into<LoggingAspect>();

            var foo = kernel.Get<Foo>();
            foo.FooMe(); 
        }
    }

    public class LoggingAspect : IAspect
    {
        public void Intercept(Castle.DynamicProxy.IInvocation invocation)
        {
            System.Console.WriteLine("Log Before");

            invocation.Proceed();

            System.Console.WriteLine("Log After");
        }
    }

    public interface IFoo
    {
        void FooMe();
    }

    public class Foo : IFoo
    {
        public void FooMe()
        {
            Console.WriteLine("FooMe");
        }
    }

}