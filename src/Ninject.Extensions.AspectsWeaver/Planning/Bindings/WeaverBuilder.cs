// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Planning.Bindings;

namespace Ninject.Extensions.AspectsWeaver.Planning.Bindings
{
    /// <summary>
    /// Provides a mechanism to decorate classes with interceptors./>.
    /// </summary>
    internal class WeaverBuilder : IPointCutOrWeaveIntoSyntax
    {
        private readonly IKernel kernel;
        private readonly IBindingConfiguration bindingConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeaverBuilder"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <param name="bindingConfiguration">The bindingConfiguration to build.</param>
        public WeaverBuilder(IKernel kernel, IBindingConfiguration bindingConfiguration)
        {
            this.kernel = kernel;
            this.bindingConfiguration = bindingConfiguration;
        }

        /// <summary>
        /// Indicates that the service should be intercepted with the specified interceptor type.
        /// </summary>
        /// <typeparam name="TAspect">The interceptor type.</typeparam>
        /// <returns>The fluent syntax.</returns>
        public IWeaveIntoSyntax Into<TAspect>()
         where TAspect : IAspect
        {
            return new PointCutsBuilder(this.kernel, this.bindingConfiguration, null)
                .Into<TAspect>();
        }

        public IWeaveIntoSyntax PointCuts(IPointCutSelector cutSelector)
        {
            return new PointCutsBuilder(this.kernel, this.bindingConfiguration, cutSelector);
        }
    }
}