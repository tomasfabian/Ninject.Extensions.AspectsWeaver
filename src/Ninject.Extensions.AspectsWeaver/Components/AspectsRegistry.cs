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
using System.Linq;
using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Planning.Bindings;

namespace Ninject.Extensions.AspectsWeaver.Components
{
    /// <summary>
    /// A component that provides interceptors.
    /// </summary>
    public class AspectsRegistry : Ninject.Components.NinjectComponent, IAspectsRegistry
    {
        private readonly IDictionary<IBindingConfiguration, IList<Type>> aspectTypes = new Dictionary<IBindingConfiguration, IList<Type>>();
        private readonly IDictionary<IBindingConfiguration, IPointCutSelector> jointPointSelectors = new Dictionary<IBindingConfiguration, IPointCutSelector>();

        /// <summary>
        /// Adds interceptor for the providerd bindingConfiguration.
        /// </summary>
        /// <param name="bindingConfiguration">The bindingConfiguration to build.</param>
        /// <param name="type">The interceptors type.</param>
        public void AddAspect(IBindingConfiguration bindingConfiguration, Type type)
        {
            IList<Type> types;
            if (this.aspectTypes.TryGetValue(bindingConfiguration, out types))
            {
                types.Add(type);
            }
            else
            {
                types = new List<Type> {type};
                this.aspectTypes[bindingConfiguration] = types;
            }
        }

        public void AddSelector(IBindingConfiguration binding, IPointCutSelector cutSelector)
        {
            this.jointPointSelectors[binding] = cutSelector;
        }

        /// <summary>
        /// Gets configured interceptors for the provided bindingConfiguration.
        /// </summary>
        /// <param name="bindingConfiguration">The bindingConfiguration to build.</param>
        /// <returns>The configured interceptor types.</returns>
        public IEnumerable<Type> GetAspectTypes(IBindingConfiguration bindingConfiguration)
        {
            IList<Type> types;
            if (this.aspectTypes.TryGetValue(bindingConfiguration, out types))
            {
                return types;
            }
            return Enumerable.Empty<Type>();
        }

        public IPointCutSelector GetSelector(IBindingConfiguration binding)
        {
            IPointCutSelector cutSelector;

            this.jointPointSelectors.TryGetValue(binding, out cutSelector);

            return cutSelector;
        }
    }
}
