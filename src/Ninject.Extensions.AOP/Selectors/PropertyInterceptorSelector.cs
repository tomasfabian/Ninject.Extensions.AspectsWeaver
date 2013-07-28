using Castle.DynamicProxy;
using System;
using System.Linq.Expressions;
using System.Reflection;
using Ninject.Extensions.AOP.Aspects;
using Ninject.Extensions.AOP.Helpers;

namespace Ninject.Extensions.AOP.Selectors
{
    public class PropertyInterceptorSelector<T> : IInterceptorSelector
    {
        private readonly string propertyName;

        public PropertyInterceptorSelector(Expression<Func<T, object>> propertyExpression)
        {
            this.propertyName = propertyExpression.GetPropertyName();
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (IsGetter(method))
            {
                return interceptors;
            }

            return InterceptionAspect.EmptyInterceptors;
        }

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
    }
}
