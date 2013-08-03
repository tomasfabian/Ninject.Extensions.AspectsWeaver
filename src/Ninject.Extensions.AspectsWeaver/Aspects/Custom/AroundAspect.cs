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
    public class AroundAspect : Aspect
    {
        private readonly Action<IBeforeArgs> beforeAction;
        private readonly Action<ISuccessArgs> afterAction;

        public AroundAspect(Action<IBeforeArgs> beforeAction, Action<ISuccessArgs> afterAction)
        {
            this.beforeAction = beforeAction;
            this.afterAction = afterAction;
        }

        protected override void Before(IBeforeArgs args)
        {
            base.Before(args);

            if (this.beforeAction != null)
            {
                this.beforeAction(args); 
            }
        }

        protected override void Success(ISuccessArgs args)
        {
            if (afterAction != null)
            {
                this.afterAction(args); 
            }

            base.Success(args);
        }
    }
}
