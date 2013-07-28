using System;

namespace Ninject.Extensions.AOP.Aspects
{
    public class ReactiveInterceptionAspect<T> : InterceptionAspect
        where T : class
    {
        readonly IObserver<T> observer;

        public ReactiveInterceptionAspect(IObserver<T> observer)
        {
            if (observer == null) throw new ArgumentNullException("observer");

            this.observer = observer;
        }

        protected override object OnSuccess(object returnValue)
        {
            var value = returnValue as T;

            this.observer.OnNext(value);

            return base.OnSuccess(returnValue);
        }

        protected override void OnException(Exception error)
        {
            this.observer.OnError(error);
        }

        protected override void OnExit()
        {
            this.observer.OnCompleted();
        }
    }
}
