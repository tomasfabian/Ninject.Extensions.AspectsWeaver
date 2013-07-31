using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Extensions.AspectsWeaver.Components;
using Ninject.Planning.Bindings;

namespace Ninject.Extensions.AspectsWeaver.Planning.Bindings
{
    public class PointCutsBuilder : IWeaveIntoSyntax
    {
        private readonly IBindingConfiguration bindingConfiguration;
        private readonly IAspectsRegistry registry;

        public PointCutsBuilder(IKernel kernel, IBindingConfiguration bindingConfiguration, IJointPointSelector selector)
        {
            this.bindingConfiguration = bindingConfiguration;

            var weaverRegistry = kernel.Components.Get<IWeaverRegistry>();
            this.registry = kernel.Components.Get<IAspectsRegistry>();

            weaverRegistry.AddRegistry(this.bindingConfiguration, this.registry);

            if (selector != null)
            {
                registry.AddSelector(bindingConfiguration, selector);
            }
        }

        public IWeaveIntoSyntax Into<TAspect>() where TAspect : IAspect
        {
            this.registry
                .AddAspect(this.bindingConfiguration, typeof(TAspect));

            return this;
        }
    }
}