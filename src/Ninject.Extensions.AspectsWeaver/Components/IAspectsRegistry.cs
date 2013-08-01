// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;
using System.Collections.Generic;
using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Planning.Bindings;

namespace Ninject.Extensions.AspectsWeaver.Components
{
    /// <summary>
    /// A component that provides aspects.
    /// </summary>
    public interface IAspectsRegistry : Ninject.Components.INinjectComponent
    {
        /// <summary>
        /// Adds interceptor for the provided bindingConfiguration.
        /// </summary>
        /// <param name="bindingConfiguration">The bindingConfiguration to build.</param>
        /// <param name="type">The interceptors type.</param>
        void AddAspect(IBindingConfiguration bindingConfiguration, Type type); 
        
        void AddSelector(IBindingConfiguration binding, IPointCutSelector cutSelector);

        /// <summary>
        /// Gets configured interceptors for the provided bindingConfiguration.
        /// </summary>
        /// <param name="bindingConfiguration">The bindingConfiguration to build.</param>
        /// <returns>The configured interceptor types.</returns>
        IEnumerable<Type> GetAspectTypes(IBindingConfiguration bindingConfiguration);

        IPointCutSelector GetSelector(IBindingConfiguration binding);
    }
}