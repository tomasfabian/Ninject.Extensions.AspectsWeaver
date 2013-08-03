// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using Castle.DynamicProxy;

namespace Ninject.Extensions.AspectsWeaver.Aspects.Custom
{
    public class InvocationAspect : IAspect
    {
        public virtual void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }
}