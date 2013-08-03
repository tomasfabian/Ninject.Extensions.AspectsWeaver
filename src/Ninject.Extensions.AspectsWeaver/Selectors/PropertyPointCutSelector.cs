// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
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
    public abstract class PropertyPointCutSelector<T> : IPointCutSelector
    {
        private readonly string propertyName;

        protected PropertyPointCutSelector(Expression<Func<T, object>> propertyExpression)
        {
            this.propertyName = propertyExpression.GetPropertyName();
        }

        public bool IsPointCut(Type type, MethodInfo methodInfo)
        {
            return methodInfo.Name == (Prefix + propertyName) && IsCorrectJointPoint(methodInfo);
        }

        protected abstract string Prefix { get;  }

        protected abstract bool IsCorrectJointPoint(MethodInfo methodInfo);
    }
}