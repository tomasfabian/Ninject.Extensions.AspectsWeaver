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
using Castle.DynamicProxy;
using Ninject;
using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Extensions.AspectsWeaver.Aspects.Contracts;
using Ninject.Extensions.AspectsWeaver.Attributes;
using Ninject.Extensions.AspectsWeaver.Modules;
using Ninject.Extensions.AspectsWeaver.Syntax;

namespace AspectWeaver.Demo.BasicHome
{
    class Program
    {
        static void Main()
        {
            WeaverDemo();
        }

        public static void WeaverDemo()
        {
            var settings = new NinjectSettings() { LoadExtensions = false };
            IKernel kernel = new StandardKernel(settings, new AspectsWeaverModule());

            kernel.Bind<IFoo>().To<Foo>()
                  .InSingletonScope()
                  .Weave()
                  .PointCuts(new FooMeSelector())
                  //.PointCuts(new DoNotFooMeSelector())
                  .Into<LoggingAspect>()
                  .Into<AdvancedLoggingAspect>();

            var foo = kernel.Get<IFoo>();
            foo.FooMe();

            foo.Dispose();
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

        [ExcludeJointPoint]
        public void Dispose()
        {
            Console.WriteLine("Foo disposed");
        }
    }

    public class LoggingAspect : IAspect
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("LoggingAspect Before");

            invocation.Proceed();

            Console.WriteLine("LoggingAspect After");
        }
    }

    public class AdvancedLoggingAspect : Aspect
    {
        protected override void Before(IBeforeArgs args)
        {
            Console.WriteLine("Log OnBefore");
        }

        protected override void Success(ISuccessArgs args)
        {
            Console.WriteLine("Log OnSuccess");
        }

        protected override void Exception(IExceptionArgs args)
        {
            Console.WriteLine("Log OnException " + args.Error.Message);
        }

        protected override void Finally(IFinallyArgs args)
        {
            Console.WriteLine("Log OnFinally");
        }
    }

    public class FooMeSelector : IPointCutSelector
    {
        public bool IsPointCut(Type type, MethodInfo method)
        {
            return method.Name == "FooMe";
        }
    }

    public class DoNotFooMeSelector : IPointCutSelector
    {
        public bool IsPointCut(Type type, MethodInfo method)
        {
            return method.Name != "FooMe";
        }
    }
}