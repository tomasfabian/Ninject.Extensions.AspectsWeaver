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
