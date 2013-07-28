using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Ninject.Planning.Bindings;

namespace Ninject.Extensions.AOP.Components
{
    /// <summary>
    /// A component that provides interceptors.
    /// </summary>
    public class InterceptionRegistry : Ninject.Components.NinjectComponent, IInterceptionRegistry
    {
        private readonly IDictionary<IBindingConfiguration, IList<Type>> interceptorTypes = new Dictionary<IBindingConfiguration, IList<Type>>();
        private readonly IDictionary<IBindingConfiguration, IInterceptorSelector> interceptorSelectors = new Dictionary<IBindingConfiguration, IInterceptorSelector>();

        /// <summary>
        /// Adds interceptor for the providerd bindingConfiguration.
        /// </summary>
        /// <param name="bindingConfiguration">The bindingConfiguration to build.</param>
        /// <param name="type">The interceptors type.</param>
        public void AddInterceptor(IBindingConfiguration bindingConfiguration, Type type)
        {
            IList<Type> types;
            if (this.interceptorTypes.TryGetValue(bindingConfiguration, out types))
            {
                types.Add(type);
            }
            else
            {
                types = new List<Type> {type};
                this.interceptorTypes[bindingConfiguration] = types;
            }
        }

        public void AddSelector(IBindingConfiguration binding, IInterceptorSelector selector)
        {
            this.interceptorSelectors[binding] = selector;
        }

        /// <summary>
        /// Gets configured interceptors for the provided bindingConfiguration.
        /// </summary>
        /// <param name="bindingConfiguration">The bindingConfiguration to build.</param>
        /// <returns>The configured interceptor types.</returns>
        public IEnumerable<Type> GetInterceptorTypes(IBindingConfiguration bindingConfiguration)
        {
            IList<Type> types;
            if (this.interceptorTypes.TryGetValue(bindingConfiguration, out types))
            {
                return types;
            }
            return Enumerable.Empty<Type>();
        }

        public IInterceptorSelector GetSelector(IBindingConfiguration binding)
        {            
            IInterceptorSelector selector;

            this.interceptorSelectors.TryGetValue(binding, out selector);

            return selector;
        }
    }
}
