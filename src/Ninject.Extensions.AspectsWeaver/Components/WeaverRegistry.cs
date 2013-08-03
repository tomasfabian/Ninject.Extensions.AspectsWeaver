// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System.Collections.Generic;
using System.Linq;
using Ninject.Components;
using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Planning.Bindings;

namespace Ninject.Extensions.AspectsWeaver.Components
{
    internal class WeaverRegistry : NinjectComponent, IWeaverRegistry
    {
        private readonly IDictionary<IBindingConfiguration, IList<IAspectsRegistry>> registriesDictionary = new Dictionary<IBindingConfiguration, IList<IAspectsRegistry>>();

        public void AddRegistry(IBindingConfiguration bindingConfiguration, IAspectsRegistry selector)
        {
            IList<IAspectsRegistry> registries;
            if (this.registriesDictionary.TryGetValue(bindingConfiguration, out registries))
            {
                registries.Add(selector);
            }
            else
            {
                registries = new List<IAspectsRegistry> { selector };
                this.registriesDictionary[bindingConfiguration] = registries;
            }
        }

        public IEnumerable<IAspectsRegistry> GetRegistry(IBindingConfiguration bindingConfiguration)
        {
            IList<IAspectsRegistry> registries;
            if (this.registriesDictionary.TryGetValue(bindingConfiguration, out registries))
            {
                return registries;
            }
            return Enumerable.Empty<IAspectsRegistry>();
        }
    }
}