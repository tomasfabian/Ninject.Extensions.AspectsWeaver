// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;
using System.Linq.Expressions;
using System.Reflection;
using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Extensions.AspectsWeaver.Helpers;

namespace Ninject.Extensions.AspectsWeaver.Selectors
{
    public class GetPropertySelector<T> : IJointPointSelector
    {
        private readonly string propertyName;

        public GetPropertySelector(Expression<Func<T, object>> propertyExpression)
        {
            this.propertyName = propertyExpression.GetPropertyName();
        }

        //public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        //{
        //    if (IsGetter(method))
        //    {
        //        return interceptors;
        //    }

        //    return Aspect.EmptyInterceptors;
        //}

        private const string SetPrefix = "set_";

        private bool IsSetter(MethodInfo methodInfo)
        {
            return CheckProperty(methodInfo, SetPrefix);
        }

        private const string GetPrefix = "get_";

        private bool IsGetter(MethodInfo methodInfo)
        {
            return CheckProperty(methodInfo, GetPrefix);
        }

        private bool CheckProperty(MethodInfo methodInfo, string prefix)
        {
            return methodInfo.Name == (prefix + propertyName) && methodInfo.IsSpecialName && methodInfo.Name.StartsWith(prefix, StringComparison.Ordinal);
        }

        public bool IsJointPoint(Type type, MethodInfo method)
        {
            return IsGetter(method);
        }
    }
}
