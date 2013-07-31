using System;
using System.Reflection;
using Castle.DynamicProxy;
using Ninject.Extensions.AspectsWeaver.Aspects;

namespace Ninject.Extensions.AspectsWeaver.Activation.Strategies
{
    internal class SelectorWithItsInterceptors : IInterceptorSelector
    {
        private readonly IInterceptor[] interceptors;
        private readonly IJointPointSelector selector;

        public SelectorWithItsInterceptors(IInterceptor[] interceptors, IJointPointSelector selector)
        {
            this.interceptors = interceptors;
            this.selector = selector;
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (this.selector.IsJointPoint(type, method))
            {
                return this.interceptors;
            }

            return Aspect.EmptyInterceptors;
        }
    }
}