// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;
using Ninject.Extensions.AspectsWeaver.Aspects.Contracts;

namespace Ninject.Extensions.AspectsWeaver.Aspects
{
    public class ReactiveAspect<T> : Aspect
        where T : class
    {
        readonly IObserver<T> observer;

        public ReactiveAspect(IObserver<T> observer)
        {
            if (observer == null) throw new ArgumentNullException("observer");

            this.observer = observer;
        }

        protected override void Success(ISuccessArgs args)
        {
            var value = args.Invocation.ReturnValue as T;

            this.observer.OnNext(value);

            base.Success(args);
        }

        protected override void Exception(IExceptionArgs args)
        {
            this.observer.OnError(args.Error);
        }

        protected override void Finally(IFinallyArgs args)
        {
            this.observer.OnCompleted();
        }
    }
}
