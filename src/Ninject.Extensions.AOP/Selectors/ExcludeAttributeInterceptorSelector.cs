// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 
using System;
using System.Linq;
using Castle.DynamicProxy;
using System.Reflection;
using Ninject.Extensions.AOP.Aspects;
using Ninject.Extensions.AOP.Attributes;
using Ninject.Extensions.AOP.Helpers;

namespace Ninject.Extensions.AOP.Selectors
{
    public class ExcludeAttributeInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            bool hasClassLevelExcludeAttribute = type.GetCustomAttributes(typeof(ExcludeFromInterceptionAttribute), true).Any();

            if (hasClassLevelExcludeAttribute)
                return InterceptionAspect.EmptyInterceptors;

            var methodFromType = type.GetMethods().FirstOrDefault(c => c.AreEqual(method));

            if (methodFromType == null)
                return InterceptionAspect.EmptyInterceptors;

            bool hasMemberLevelExcludeAttribute = methodFromType.GetCustomAttributes(typeof(ExcludeFromInterceptionAttribute), true).Any();

            return hasMemberLevelExcludeAttribute ? InterceptionAspect.EmptyInterceptors : interceptors;
        }
    }
}
