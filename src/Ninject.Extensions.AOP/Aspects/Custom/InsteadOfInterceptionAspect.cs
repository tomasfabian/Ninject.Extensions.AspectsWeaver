// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 
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

        protected override object OnSuccess(object[] arguments, object returnValue)
        {
            return this.insteadOfAction(returnValue);
        }
    }
}
