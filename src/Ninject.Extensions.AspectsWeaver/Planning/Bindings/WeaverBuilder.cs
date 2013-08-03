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
        /// <param name="bindingConfiguration">The binding configuration to build.</param>
        public WeaverBuilder(IKernel kernel, IBindingConfiguration bindingConfiguration)
        {
            this.kernel = kernel;
            this.bindingConfiguration = bindingConfiguration;
        }

        /// <summary>
        /// Indicates that the service should be intercepted with the specified aspect type.
        /// </summary>
        /// <typeparam name="TAspect">The aspect's type.</typeparam>
        /// <returns>The weaver builder's fluent syntax.</returns>
        public IWeaveIntoSyntax Into<TAspect>()
         where TAspect : IAspect
        {
            return new PointCutsBuilder(this.kernel, this.bindingConfiguration, null)
                .Into<TAspect>();
        }

        /// <summary>
        /// Indicates that the interception should be limited to selected pointcuts.
        /// </summary>
        /// <param name="pointcutSelector">The pointcuts selector.</param>
        /// <returns>The weaver builder's fluent syntax.</returns>
        public IWeaveIntoSyntax PointCuts(IPointCutSelector pointcutSelector)
        {
            return new PointCutsBuilder(this.kernel, this.bindingConfiguration, pointcutSelector);
        }
    }
}