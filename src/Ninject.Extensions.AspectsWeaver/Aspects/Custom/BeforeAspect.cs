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
    public class BeforeAspect : AroundAspect
    {
        public BeforeAspect()
            : base(args => { }, null)
        {
            
        }

        public BeforeAspect(Action<IBeforeArgs> beforeAction) 
            : base(beforeAction, null)
        {
        }

        protected sealed override void Success(ISuccessArgs args)
        {
            base.Success(args);
        }
    }
}