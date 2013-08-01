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
using Castle.DynamicProxy;
using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Extensions.AspectsWeaver.Helpers;

namespace Ninject.Extensions.AspectsWeaver.Selectors
{
    public class GetPropertyCutSelector<T> : IPointCutSelector
    {
        private readonly string propertyName;

        public GetPropertyCutSelector(Expression<Func<T, object>> propertyExpression)
        {
            this.propertyName = propertyExpression.GetPropertyName();
        }

        public bool IsPointCut(Type type, MethodInfo methodInfo)
        {
            return methodInfo.Name == (ReflectionExtensions.GetPrefix + propertyName) && methodInfo.IsGetter();
        }
    }

    public class GetPropertyInterceptorSelector<T> : IInterceptorSelector
    {
        private readonly IPointCutSelector cutSelector;

        public GetPropertyInterceptorSelector(Expression<Func<T, object>> propertyExpression)
        {
            this.cutSelector = new GetPropertyCutSelector<T>(propertyExpression);
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (this.cutSelector.IsPointCut(type, method))
            {
                return interceptors;
            }

            return Aspect.EmptyInterceptors;
        }
    }
}
