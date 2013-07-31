using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;

namespace Ninject.Extensions.AspectsWeaver.Selectors
{
    public class CompositeSelector : IInterceptorSelector
    {
        private readonly IList<IInterceptorSelector> selectors;

        public CompositeSelector(IEnumerable<IInterceptorSelector> selectors)
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