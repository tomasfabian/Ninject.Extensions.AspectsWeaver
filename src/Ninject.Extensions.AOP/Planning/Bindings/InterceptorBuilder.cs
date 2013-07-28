// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 
using Ninject.Extensions.AOP.Components;
using Castle.DynamicProxy;
using Ninject.Planning.Bindings;

namespace Ninject.Extensions.AOP.Planning.Bindings
{
    /// <summary>
    /// Provides a mechanism to decorate classes with interceptors./>.
    /// </summary>
    public class InterceptorBuilder : IInterceptorOrSelectorSyntax
    {
        private readonly IKernel kernel;
        private readonly IBindingConfiguration bindingConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptorBuilder"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <param name="bindingConfiguration">The bindingConfiguration to build.</param>
        public InterceptorBuilder(IKernel kernel, IBindingConfiguration bindingConfiguration)
        {
            this.kernel = kernel;
            this.bindingConfiguration = bindingConfiguration;
        }

        /// <summary>
        /// Indicates that the service should be intercepted with the specified interceptor type.
        /// </summary>
        /// <typeparam name="TInterceptor">The interceptor type.</typeparam>
        /// <returns>The fluent syntax.</returns>
        public IInterceptorOrSelectorSyntax With<TInterceptor>()
         where TInterceptor : IInterceptor
        {
            this.kernel.Components.Get<IInterceptionRegistry>()
            .AddInterceptor(this.bindingConfiguration, typeof(TInterceptor));

            return this;
        }

        public void FilterInterceptionWith(IInterceptorSelector selector)
        {
            this.kernel.Components.Get<IInterceptionRegistry>()
                .AddSelector(this.bindingConfiguration, selector);
        }
    }
}