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

namespace Ninject.Extensions.AspectsWeaver.Aspects.Custom
{
    public class InsteadOfAspect : Aspect
    {
        readonly Func<ISuccessArgs, object> insteadOfAction;

        public InsteadOfAspect()
            : this(args => null)
        {
            
        }

        public InsteadOfAspect(Func<ISuccessArgs, object> insteadOfAction)
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

        protected sealed override void Before(IBeforeArgs args)
        {
            base.Before(args);
        }

        protected override void Success(ISuccessArgs args)
        {
            args.Invocation.ReturnValue = this.insteadOfAction(args);
        }

        protected sealed override void Exception(IExceptionArgs args)
        {
            base.Exception(args);
        }

        protected sealed override void Finally(IFinallyArgs args)
        {
            base.Finally(args);
        }
    }
}
