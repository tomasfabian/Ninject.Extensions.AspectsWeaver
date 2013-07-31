// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System.Collections.Generic;
using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Planning.Bindings;

namespace Ninject.Extensions.AspectsWeaver.Components
{
    public interface IJointPointsRegistry : Ninject.Components.INinjectComponent
    {
        void AddSelector(IBindingConfiguration bindingConfiguration, IJointPointSelector selector);
        IEnumerable<IJointPointSelector> GetSelector(IBindingConfiguration bindingConfiguration);
    }
}