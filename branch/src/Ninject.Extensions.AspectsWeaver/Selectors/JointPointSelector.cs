// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;
using System.Reflection;
using Castle.DynamicProxy;
using Ninject.Extensions.AspectsWeaver.Aspects;

namespace Ninject.Extensions.AspectsWeaver.Selectors
{
    public class JointPointSelector : IInterceptorSelector
    {
        private readonly IJointPointSelector selector;

        public JointPointSelector(IJointPointSelector selector)
        {
            this.selector = selector;
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (this.selector.IsJointPoint(type, method))
            {
                return interceptors;
            }

            return Aspect.EmptyInterceptors;
        } 
    }
}