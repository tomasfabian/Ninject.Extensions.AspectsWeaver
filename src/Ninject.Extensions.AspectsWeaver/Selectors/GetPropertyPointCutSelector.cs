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
    public class GetPropertyPointCutSelector<T> : PropertyPointCutSelector<T>
    {
        public GetPropertyPointCutSelector(Expression<Func<T, object>> propertyExpression)
            : base(propertyExpression)
        {
        }

        protected override string Prefix { get { return ReflectionExtensions.GetPrefix; } }

        protected override bool IsCorrectJointPoint(MethodInfo methodInfo)
        {
            return methodInfo.IsGetter();
        }
    }

    public class GetPropertyInterceptorSelector<T> : IInterceptorSelector
    {
        private readonly IPointCutSelector cutSelector;

        public GetPropertyInterceptorSelector(Expression<Func<T, object>> propertyExpression)
        {
            this.cutSelector = new GetPropertyPointCutSelector<T>(propertyExpression);
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
