// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;

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

        protected override object OnSuccess(object[] arguments, object returnValue)
        {
            var value = returnValue as T;

            this.observer.OnNext(value);

            return base.OnSuccess(arguments, returnValue);
        }

        protected override void OnException(object[] arguments, Exception error)
        {
            this.observer.OnError(error);
        }

        protected override void OnExit(object[] arguments)
        {
            this.observer.OnCompleted();
        }
    }
}
