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
using Ninject.Extensions.AspectsWeaver.Helpers;

namespace Ninject.Extensions.AspectsWeaver.Selectors
{
    public class SetPropertyPointCutSelector<T> : PropertyPointCutSelector<T>
    {
        public SetPropertyPointCutSelector(Expression<Func<T, object>> propertyExpression)
            : base(propertyExpression)
        {
        }

        protected override string Prefix { get { return ReflectionExtensions.SetPrefix; } }

        protected override bool IsCorrectJointPoint(MethodInfo methodInfo)
        {
            return methodInfo.IsSetter();
        }
    }
}