// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System.Collections.Generic;
using Ninject.Planning.Bindings;

namespace Ninject.Extensions.AspectsWeaver.Components
{
    public interface IWeaverRegistry : Ninject.Components.INinjectComponent
    {
        void AddRegistry(IBindingConfiguration bindingConfiguration, IAspectsRegistry selector);
        IEnumerable<IAspectsRegistry> GetRegistry(IBindingConfiguration bindingConfiguration);
    }
}