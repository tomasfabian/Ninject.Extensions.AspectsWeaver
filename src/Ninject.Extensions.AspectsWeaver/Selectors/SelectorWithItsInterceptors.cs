using System;
using System.Reflection;
using Castle.DynamicProxy;
using Ninject.Extensions.AspectsWeaver.Aspects;

namespace Ninject.Extensions.AspectsWeaver.Activation.Strategies
{
    internal class SelectorWithItsInterceptors : IInterceptorSelector
    {
        private readonly IInterceptor[] interceptors;
        private readonly IPointCutSelector cutSelector;

        public SelectorWithItsInterceptors(IInterceptor[] interceptors, IPointCutSelector cutSelector)
        {
            this.interceptors = interceptors;
            this.cutSelector = cutSelector;
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (this.cutSelector.IsPointCut(type, method))
            {
                return this.interceptors;
            }

            return Aspect.EmptyInterceptors;
        }
    }
}