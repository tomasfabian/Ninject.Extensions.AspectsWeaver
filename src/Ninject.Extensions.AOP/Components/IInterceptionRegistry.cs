using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Ninject.Planning.Bindings;

namespace Ninject.Extensions.AOP.Components
{
    /// <summary>
    /// A component that provides interceptors.
    /// </summary>
    public interface IInterceptionRegistry : Ninject.Components.INinjectComponent
    {
        /// <summary>
        /// Adds interceptor for the provided bindingConfiguration.
        /// </summary>
        /// <param name="bindingConfiguration">The bindingConfiguration to build.</param>
        /// <param name="type">The interceptors type.</param>
        void AddInterceptor(IBindingConfiguration bindingConfiguration, Type type); 
        
        void AddSelector(IBindingConfiguration binding, IInterceptorSelector selector);

        /// <summary>
        /// Gets configured interceptors for the provided bindingConfiguration.
        /// </summary>
        /// <param name="bindingConfiguration">The bindingConfiguration to build.</param>
        /// <returns>The configured interceptor types.</returns>
        IEnumerable<Type> GetInterceptorTypes(IBindingConfiguration bindingConfiguration);

        IInterceptorSelector GetSelector(IBindingConfiguration binding);
    }
}
