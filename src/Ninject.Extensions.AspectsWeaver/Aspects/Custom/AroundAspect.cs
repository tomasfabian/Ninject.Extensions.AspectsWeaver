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
        private Action<IBeforeArgs> BeforeAction { get; set; }
        private Action<ISuccessArgs> AfterAction { get; set; }

        public AroundAspect()
            : this(args => { }, args => { })
        {
            
        }

        public AroundAspect(Action<IBeforeArgs> beforeAction, Action<ISuccessArgs> afterAction)
        {
            this.BeforeAction = beforeAction;
            this.AfterAction = afterAction;
        }

        protected override void Before(IBeforeArgs args)
        {
            base.Before(args);

            if (this.BeforeAction != null)
            {
                this.BeforeAction(args); 
            }
        }

        protected override void Success(ISuccessArgs args)
        {
            if (AfterAction != null)
            {
                this.AfterAction(args); 
            }

            base.Success(args);
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
