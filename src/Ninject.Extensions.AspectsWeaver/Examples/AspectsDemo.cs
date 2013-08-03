// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;
using AspectWeaver.Demo.Basic;
using Castle.DynamicProxy;
using Ninject;
using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Extensions.AspectsWeaver.Aspects.Contracts;
using Ninject.Extensions.AspectsWeaver.Aspects.Custom;
using Ninject.Extensions.AspectsWeaver.Modules;
using Ninject.Extensions.AspectsWeaver.Planning.Bindings;

namespace AspectWeaver.Demo.Aspects
{
    class Program
    {
        private static void Main()
        {
            InterceptGet();
        }

        public void InterceptGet()
        {
            Ninject.IKernel kernel = new Ninject.StandardKernel(new AspectsWeaverModule());

            kernel.Bind<IFoo>().To<Foo>()
            .Weave()
                .Into<LoggingBeforeAspect>()
                //.Into<LoggingAfterAspect>(
                //.Into<LoggingAroundAspect>()
                //.Into<LoggingInsteadOfAspect>()
                //.Into<LoggingInvocationAspect>()
                //.Into<LoggingAspect>()
                ;

            var instance = kernel.Get<IFoo>();
            instance.FooMe();
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

    public class LoggingBeforeAspect : BeforeAspect
    {
        public LoggingBeforeAspect()
            : this(beforeAction: args => Console.WriteLine("Log Before"))
        {

        }

        public LoggingBeforeAspect(Action<IBeforeArgs> beforeAction)
            : base(beforeAction)
        {

        }
    }
    //Output:
    //Log Before
    //FooMe

    public class LoggingAfterAspect : AfterAspect
    {
        public LoggingAfterAspect()
            : this(afterAction: args => Console.WriteLine("Log After"))
        {

        }

        public LoggingAfterAspect(Action<ISuccessArgs> afterAction)
            : base(afterAction)
        {

        }
    }
    //Output:
    //FooMe
    //Log After


    public class LoggingAroundAspect : AroundAspect
    {
        public LoggingAroundAspect()
            : this(beforeAction: args => Console.WriteLine("Log Before"), afterAction: args => Console.WriteLine("Log After"))
        {

        }

        public LoggingAroundAspect(Action<IBeforeArgs> beforeAction, Action<ISuccessArgs> afterAction)
            : base(beforeAction, afterAction)
        {

        }
    }
    //Output:
    //Log Before
    //FooMe
    //Log After

    //Origianal method will not be called. You can set the result value in which will be returned in case of functions. 
    public class LoggingInsteadOfAspect : InsteadOfAspect
    {
        public LoggingInsteadOfAspect()
            : this(insteadOfAction: args => { 
                Console.WriteLine("Instead of");    
                return null;
            })
        {

        }

        public LoggingInsteadOfAspect(Func<ISuccessArgs, object> insteadOfAction)
            : base(insteadOfAction)
        {

        }
    }
    //Output:
    //Instead of

    public class LoggingInvocationAspect : InvocationAspect
    {
        public override void Intercept(IInvocation invocation)
        {
            try
            {
                Console.WriteLine("Log OnBefore");

                invocation.Proceed();

                Console.WriteLine("Log OnBefore");
            }
            catch(Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }
    }
    //Output:
    //Log OnBefore
    //FooMe
    //Log OnBefore

    public class LoggingAspect : Aspect
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

    //Output:
    //Log OnBefore
    //FooMe
    //Log OnSuccess
    //Log OnFinally
}