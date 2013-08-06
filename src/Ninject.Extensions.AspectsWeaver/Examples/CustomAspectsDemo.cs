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
using Ninject.Extensions.AspectsWeaver.Aspects.Contracts;
using Ninject.Extensions.AspectsWeaver.Aspects.Custom;
using Ninject.Extensions.AspectsWeaver.Modules;
using Ninject.Extensions.AspectsWeaver.Syntax;

namespace AspectWeaver.Demo.CustomAspects
{
    class Program
    {
        private static void Main()
        {
            InterceptGetterAndSetter();
        }

        public static void InterceptGetterAndSetter()
        {
            Ninject.IKernel kernel = new Ninject.StandardKernel(new AspectsWeaverModule());

            var binding = kernel.Bind<IFoo>().To<Foo>();

            binding
            .WeaveGetProperty(foo => foo.FooProperty)
            .Into<LoggingGetPropertyAspect>();

            binding
            .WeaveSetProperty(foo => foo.FooProperty)
            .Into<LoggingSetPropertyAspect>();

            var instance = kernel.Get<IFoo>();

            int result = instance.FooProperty;
            instance.FooProperty = 2;
        }
    }

    public interface IFoo
    {
        int FooProperty { get; set; }
    }

    public class Foo : IFoo
    {
        public int FooProperty { get; set; }
    }

    public sealed class LoggingGetPropertyAspect : AfterAspect
    {
        protected override void Success(ISuccessArgs args)
        {
            base.Success(args);

            Console.WriteLine(string.Format("Log get value: {0}", args.Invocation.ReturnValue));
        }
    }

    public sealed class LoggingSetPropertyAspect : AfterAspect
    {
        protected override void Success(ISuccessArgs args)
        {
            base.Success(args);

            Console.WriteLine(string.Format("Log set value: {0}", args.Invocation.GetArgumentValue(0)));
        }
    }

    //Output:
    //Log get value: 0
    //Log set value: 2
}