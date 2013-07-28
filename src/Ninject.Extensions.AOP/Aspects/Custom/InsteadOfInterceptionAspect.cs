using System;

namespace Ninject.Extensions.AOP.Aspects.Custom
{
    public class InsteadOfInterceptionAspect : InterceptionAspect
    {
        readonly Func<object, object> insteadOfAction;

        public InsteadOfInterceptionAspect(Func<object, object> insteadOfAction)
        {
            if (insteadOfAction == null) throw new ArgumentNullException("insteadOfAction");
            this.insteadOfAction = insteadOfAction;
        }

        protected override bool RunInstead
        {
            get
            {
                return true;
            }
        }

        protected override object OnSuccess(object returnValue)
        {
            return this.insteadOfAction(returnValue);
        }
    }
}
