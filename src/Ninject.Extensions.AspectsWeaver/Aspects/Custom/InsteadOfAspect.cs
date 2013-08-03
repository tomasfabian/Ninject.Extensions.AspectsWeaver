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

        protected override void Success(ISuccessArgs args)
        {
            args.Invocation.ReturnValue = this.insteadOfAction(args);
        }
    }
}
