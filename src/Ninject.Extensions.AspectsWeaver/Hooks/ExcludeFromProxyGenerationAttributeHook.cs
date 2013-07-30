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
using Ninject.Extensions.AspectsWeaver.Attributes;

namespace Ninject.Extensions.AspectsWeaver.Hooks
{
    internal class ExcludeFromProxyGenerationAttributeHook : ProxyGenerationHook
    {
        protected override bool OnShouldInterceptMethod(Type type, System.Reflection.MethodInfo memberInfo)
        {
            bool hasClassLevelExcludeAttribute = type.GetCustomAttributes(typeof(ExcludeJointPointAttribute), true).Any();

            if (hasClassLevelExcludeAttribute)
                return false;

            bool hasMemberLevelExcludeAttribute = memberInfo.GetCustomAttributes(typeof(ExcludeJointPointAttribute), true).Any();

            return !hasMemberLevelExcludeAttribute;
        }
    }
}
