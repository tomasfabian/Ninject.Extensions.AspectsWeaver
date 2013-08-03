// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;
using System.Reflection;
using AspectWeaver.Demo.Basic;
using Ninject;
using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Extensions.AspectsWeaver.Modules;
using Ninject.Extensions.AspectsWeaver.Planning.Bindings;

namespace AspectWeaver.Demo.Selectors
{
    internal class Program
    {
        private static void Main()
        {

        }

        public void InterceptGet()
        {
            Ninject.IKernel kernel = new Ninject.StandardKernel(new AspectsWeaverModule());

            kernel.Bind<IFoo>().To<Foo>()
            .WeaveGetProperty(foo => foo.FooGetter)
            .Into<LoggingAspect>();

            var instance = kernel.Get<IFoo>();
            int result = instance.FooGetter;
            instance.FooMe();

            Console.WriteLine("Result: " + result);
        }
    }

    public interface IFoo
    {
        void FooMe();
        int FooGetter { get; }
    }

    public class Foo : IFoo
    {
        public void FooMe()
        {
            Console.WriteLine("FooMe");
        }

        public int FooGetter { get { return 1; } }
    }

    public class LoggingAspect : IAspect
    {
        public void Intercept(Castle.DynamicProxy.IInvocation invocation)
        {
            Console.WriteLine("Log Before Get");

            invocation.Proceed();

            Console.WriteLine("Log After Get");
        }
    }

    public class CustomSelector : IPointCutSelector
    {
        public bool IsPointCut(Type type, MethodInfo method)
        {
            return method.Name != "Dispose";
        }
    }
}