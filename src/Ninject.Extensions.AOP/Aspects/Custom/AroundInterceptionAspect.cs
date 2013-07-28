using System;

namespace Ninject.Extensions.AOP.Aspects.Custom
{
    public class AroundInterceptionAspect : InterceptionAspect
    {
        private readonly Action<object[]> beforeAction;
        private readonly Action<object> afterAction;

        public AroundInterceptionAspect(Action<object[]> beforeAction, Action<object> afterAction)
        {
            this.beforeAction = beforeAction;
            this.afterAction = afterAction;
        }

        protected override void OnEntry(object[] arguments)
        {
            base.OnEntry(arguments);

            if (this.beforeAction != null)
            {
                this.beforeAction(arguments); 
            }
        }

        protected override object OnSuccess(object returnValue)
        {
            if (afterAction != null)
            {
                this.afterAction(returnValue); 
            }

            return base.OnSuccess(returnValue);
        }
    }
}
