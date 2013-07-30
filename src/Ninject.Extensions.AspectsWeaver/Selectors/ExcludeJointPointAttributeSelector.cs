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
using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Extensions.AspectsWeaver.Attributes;
using Ninject.Extensions.AspectsWeaver.Helpers;

namespace Ninject.Extensions.AspectsWeaver.Selectors
{
    public class ExcludeJointPointAttributeSelector : IJointPointSelector
    {
        //public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        //{
        //    bool hasClassLevelExcludeAttribute = type.GetCustomAttributes(typeof(ExcludeJointPointAttribute), true).Any();

        //    if (hasClassLevelExcludeAttribute)
        //        return Aspect.EmptyInterceptors;

        //    var methodFromType = type.GetMethods().FirstOrDefault(c => c.AreEqual(method));

        //    if (methodFromType == null)
        //        return Aspect.EmptyInterceptors;

        //    bool hasMemberLevelExcludeAttribute = methodFromType.GetCustomAttributes(typeof(ExcludeJointPointAttribute), true).Any();

        //    return hasMemberLevelExcludeAttribute ? Aspect.EmptyInterceptors : interceptors;
        //}

        public bool IsJointPoint(Type type, MethodInfo method)
        {
            bool hasClassLevelExcludeAttribute = type.GetCustomAttributes(typeof(ExcludeJointPointAttribute), true).Any();

            if (hasClassLevelExcludeAttribute)
                return false;

            var methodFromType = type.GetMethods().FirstOrDefault(c => c.AreEqual(method));

            if (methodFromType == null)
                return false;

            bool hasMemberLevelExcludeAttribute = methodFromType.GetCustomAttributes(typeof(ExcludeJointPointAttribute), true).Any();

            return !hasMemberLevelExcludeAttribute;
        }
    }
}
