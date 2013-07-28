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
using Ninject.Extensions.AOP.Attributes;

namespace Ninject.Extensions.AOP.Hooks
{
    public class ExcludeFromProxyGenerationAttributeHook : ProxyGenerationHook
    {
        protected override bool OnShouldInterceptMethod(Type type, System.Reflection.MethodInfo memberInfo)
        {
            bool hasClassLevelExcludeAttribute = type.GetCustomAttributes(typeof(ExcludeFromInterceptionAttribute), true).Any();

            if (hasClassLevelExcludeAttribute)
                return false;

            bool hasMemberLevelExcludeAttribute = memberInfo.GetCustomAttributes(typeof(ExcludeFromInterceptionAttribute), true).Any();

            return !hasMemberLevelExcludeAttribute;
        }
    }
}
