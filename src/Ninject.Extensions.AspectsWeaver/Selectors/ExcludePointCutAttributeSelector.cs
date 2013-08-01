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
using System.Reflection;
using Castle.DynamicProxy;
using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Extensions.AspectsWeaver.Attributes;
using Ninject.Extensions.AspectsWeaver.Helpers;

namespace Ninject.Extensions.AspectsWeaver.Selectors
{
    public class ExcludeJointPointAttributeInterceptorSelector : IInterceptorSelector
    {
        private readonly IPointCutSelector cutSelector;

        public ExcludeJointPointAttributeInterceptorSelector()
        {
            this.cutSelector = new ExcludePointCutAttributeSelector();
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            bool doesNotHaveMemberLevelExcludeAttribute = this.cutSelector.IsPointCut(type, method);

            return doesNotHaveMemberLevelExcludeAttribute ? interceptors : Aspect.EmptyInterceptors;
        }
    }

    internal class ExcludePointCutAttributeSelector : IPointCutSelector
    {
        public bool IsPointCut(Type type, MethodInfo method)
        {
            bool hasClassLevelExcludeAttribute = type.GetCustomAttributes(typeof(ExcludeJointPointAttribute), true).Any();

            if (hasClassLevelExcludeAttribute)
                return false;

            MemberInfo memberInfo;
            if (method.IsGetter() || method.IsSetter())
            {
                memberInfo = type.GetProperty(method.Name.Remove(0, ReflectionExtensions.GetPrefix.Length));
            }
            else
            {
                memberInfo = type.GetMethods().FirstOrDefault(c => c.AreEqual(method));
            }

            if (memberInfo == null)
                return false;

            bool hasMemberLevelExcludeAttribute = memberInfo.GetCustomAttributes(typeof(ExcludeJointPointAttribute), true).Any();

            return !hasMemberLevelExcludeAttribute;
        }
    }
}
