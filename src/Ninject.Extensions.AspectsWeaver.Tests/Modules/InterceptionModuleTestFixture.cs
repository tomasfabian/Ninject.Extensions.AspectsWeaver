using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject.Activation.Strategies;
using Ninject.Extensions.AspectsWeaver.Activation.Strategies;
using Ninject.Extensions.AspectsWeaver.Components;
using Ninject.Extensions.AspectsWeaver.Modules;
using Ninject.Modules;

namespace Ninject.Extensions.AspectsWeaver.Tests.Modules
{
    [TestClass]
    public class InterceptionModuleTestFixture
    {
        private NinjectModule interceptionModule;
        private IKernel kernel;

        [TestInitialize]
        public void Initialize()
        {
            this.interceptionModule = new AspectsWeaverModule();
        }

        [TestCleanup]
        public void CleanUp()
        {
            this.kernel.Dispose();
        }

        [TestMethod]
        public void Load_InterceptActivationStrategyHasBeenAddedToComponents()
        {
            //Arrange
            var settings = new NinjectSettings() { LoadExtensions = false };

            //Act
            this.kernel = new StandardKernel(settings, this.interceptionModule);

            //Assert
            var activationStrategy = this.kernel.Components.GetAll<IActivationStrategy>().OfType<AspectsWeaverActivationStrategy>().FirstOrDefault();

            Assert.IsNotNull(activationStrategy);
        }

        [TestMethod]
        public void Load_InterceptionRegistryHasBeenAddedToComponents()
        {
            //Arrange
            var settings = new NinjectSettings() { LoadExtensions = false };

            //Act
            this.kernel = new StandardKernel(settings, this.interceptionModule);

            //Assert
            var interceptionRegistry = this.kernel.Components.Get<IAspectsRegistry>();

            Assert.IsNotNull(interceptionRegistry);
        }

        [TestMethod]
        public void Load_CanLoadExtensions_InterceptionRegistryHasBeenAddedToComponents()
        {
            //Arrange
            var settings = new NinjectSettings() { LoadExtensions = true };

            //Act
            this.kernel = new StandardKernel(settings);

            //Assert
            var interceptionRegistry = this.kernel.Components.Get<IAspectsRegistry>();

            Assert.IsNotNull(interceptionRegistry);
        }
    }
}