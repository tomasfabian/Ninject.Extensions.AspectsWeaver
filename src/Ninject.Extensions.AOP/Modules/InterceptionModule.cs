// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 
using Ninject.Extensions.AOP.Activation.Strategies;
using Ninject.Extensions.AOP.Components;
using Ninject.Modules;
using Ninject.Activation.Strategies;

namespace Ninject.Extensions.AOP.Modules
{
    /// <summary>
    /// A loadable unit that emables interception for injected instances.
    /// </summary>
    public class InterceptionModule : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            this.Kernel.Components.Add<IActivationStrategy, InterceptActivationStrategy>();
            this.Kernel.Components.Add<IInterceptionRegistry, InterceptionRegistry>();
        }
    }
}
