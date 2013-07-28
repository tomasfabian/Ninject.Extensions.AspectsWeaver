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
using System.Linq.Expressions;
using System.Reflection;

namespace Ninject.Extensions.AOP.Helpers
{
    public static class ReflectionExtensions
    {
        #region GetPropertyName

        public static string GetPropertyName<T>(this Expression<Func<T, object>> propertyExpression)
        {
            var body = propertyExpression.Body;

            var convertExpresion = propertyExpression.Body as UnaryExpression;
            if (convertExpresion != null && propertyExpression.Body.NodeType == ExpressionType.Convert)
            {
                body = convertExpresion.Operand;
            }

            var memberExpression = body as MemberExpression;

            if (memberExpression == null || (memberExpression.Member as PropertyInfo == null))
            {
                throw new InvalidOperationException("Expression must be a property getter");
            }

            var methodInfo = memberExpression.Member as PropertyInfo;

            if (methodInfo != null)
            {
                return methodInfo.Name;
            }

            return string.Empty;
        }

        #endregion

        #region AreEqual

        public static bool AreEqual(this MethodInfo left, MethodInfo right)
        {
            if (left.Equals(right))
                return true;

            ParameterInfo[] leftParams = left.GetParameters();
            ParameterInfo[] rightParams = right.GetParameters();

            if (leftParams.Length != rightParams.Length)
                return false;

            if (leftParams.Where((t, i) => t.ParameterType != rightParams[i].ParameterType).Any())
            {
                return false;
            }

            if (left.ReturnType != right.ReturnType)
                return false;

            var leftGenericArguments = left.GetGenericArguments();
            var rightGenericArguments = right.GetGenericArguments();

            if (leftGenericArguments.Length != rightGenericArguments.Length)
                return false;

            return left.Name == right.Name;
        }

        #endregion
    }
}