using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Ninject.Extensions.AspectsWeaver.Activation.Strategies;

namespace Ninject.Extensions.AspectsWeaver.Selectors
{
    internal class JoinableCompositeSelector : IInterceptorSelector
    {
        private readonly IList<SelectorWithItsInterceptors> selectors;

        public JoinableCompositeSelector(IEnumerable<SelectorWithItsInterceptors> selectors)
        {
            this.selectors = new List<SelectorWithItsInterceptors>(selectors);
        }

        public IInterceptor[] SelectInterceptors(Type type, System.Reflection.MethodInfo method, IInterceptor[] interceptors)
        {
            return this.selectors.SelectMany(c => c.SelectInterceptors(type, method, interceptors)).ToArray();
        }
    }
}