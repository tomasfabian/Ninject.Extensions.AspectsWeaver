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
    public class BeforeInterceptionAspect : AroundInterceptionAspect
    {
        public BeforeInterceptionAspect(Action<object[]> beforeAction) 
            : base(beforeAction, null)
        {
        }
    }
}