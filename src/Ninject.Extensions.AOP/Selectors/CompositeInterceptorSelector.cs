using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ninject.Extensions.AOP.Selectors
{
    public class CompositeInterceptorSelector : IInterceptorSelector
    {
        private readonly IList<IInterceptorSelector> selectors;

        public CompositeInterceptorSelector(IEnumerable<IInterceptorSelector> selectors)
        {
            this.selectors = new List<IInterceptorSelector>(selectors);
        }

        public IInterceptor[] SelectInterceptors(Type type, System.Reflection.MethodInfo method, IInterceptor[] interceptors)
        {
            IInterceptor[] filteredInterceptors = interceptors.ToArray();

            foreach (var selector in this.selectors)
            {
                filteredInterceptors = selector.SelectInterceptors(type, method, filteredInterceptors);
            }

            return filteredInterceptors;
        }
    }
}
