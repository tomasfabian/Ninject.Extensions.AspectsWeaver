// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;
using System.Collections.Generic;
using System.Linq;
using Ninject.Components;
using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Planning.Bindings;

namespace Ninject.Extensions.AspectsWeaver.Components
{
    public class JointPointsRegistry : NinjectComponent, IJointPointsRegistry
    {
        private readonly IDictionary<IBindingConfiguration, IList<IJointPointSelector>> selectors = new Dictionary<IBindingConfiguration, IList<IJointPointSelector>>();

        public void AddSelector(IBindingConfiguration bindingConfiguration, IJointPointSelector selector)
        {
            IList<IJointPointSelector> types;
            if (this.selectors.TryGetValue(bindingConfiguration, out types))
            {
                types.Add(selector);
            }
            else
            {
                types = new List<IJointPointSelector> { selector };
                this.selectors[bindingConfiguration] = types;
            }
        }

        public IEnumerable<IJointPointSelector> GetSelector(IBindingConfiguration bindingConfiguration)
        {
            IList<IJointPointSelector> selectors;
            if (this.selectors.TryGetValue(bindingConfiguration, out selectors))
            {
                return selectors;
            }
            return Enumerable.Empty<IJointPointSelector>();
        }
    }
}