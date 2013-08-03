using Castle.DynamicProxy;
using Ninject.Extensions.AspectsWeaver.Selectors;
using Ninject.Extensions.AspectsWeaver.Attributes;
using System;
using System.Reflection;
using Ninject;
using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Extensions.AspectsWeaver.Modules;
using Ninject.Extensions.AspectsWeaver.Planning.Bindings;
using Ninject.Extensions.AspectsWeaver.Aspects.Custom;
using Ninject.Extensions.AspectsWeaver.Aspects.Contracts;

//Assenbly dependencies
//Castle.Core.dll
//Ninject.dll
//Ninject.Extensions.AspectsWeaver.dll

namespace AspectWeaver.Demo
{
    class Program
    {
        static void Main()
        {
            WeaverDemo();
        }

        public static void WeaverDemo()
        {
            var kernel = new StandardKernel(new AspectsWeaverModule());

            var multiFooBindingSyntax = kernel.Bind<IMultiFoo>().To<MultiFoo>()
                         .InSingletonScope();

            var syntax = multiFooBindingSyntax.Weave();

            syntax.PointCuts(new Foo2Selector())
                .Into<LoggingAspect1>()
                .Into<LoggingAspect2>();

            syntax.PointCuts(new Foo23Selector())
                .Into<LoggingAspect1>();

            var foo = kernel.Get<IMultiFoo>();
            foo.Foo1();
            foo.Foo2();
            foo.Foo3();
        }
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
            Console.WriteLine("Foo1");
        }

        public void Foo2()
        {
            Console.WriteLine("Foo2");
        }

        public void Foo3()
        {
            Console.WriteLine("Foo3");
        }
    }

    public class Foo2Selector : IPointCutSelector
    {
        public bool IsPointCut(Type type, MethodInfo method)
        {
            return method.Name == "Foo2";
        }
    }

    public class Foo23Selector : IPointCutSelector
    {
        public bool IsPointCut(Type type, MethodInfo method)
        {
            return method.Name == "Foo2" || method.Name == "Foo3";
        }
    }

    public class LoggingAspect1 : InvocationAspect
    {
        public override void Intercept(IInvocation invocation)
        {
            Console.WriteLine("Log1 Before");

            invocation.Proceed();

            Console.WriteLine("Log1 After");
        }
    }

    public class LoggingAspect2 : AroundAspect
    {
        public LoggingAspect2()
            : this(beforeAction: args => Console.WriteLine("Log2 Before"), afterAction: args => Console.WriteLine("Log2 After"))
        {

        }

        public LoggingAspect2(Action<IBeforeArgs> beforeAction, Action<ISuccessArgs> afterAction)
            : base(beforeAction, afterAction)
        {

        }
    }
}
