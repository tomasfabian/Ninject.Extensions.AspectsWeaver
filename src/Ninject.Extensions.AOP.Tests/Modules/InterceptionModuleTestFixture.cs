using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject.Activation.Strategies;
using Ninject.Extensions.AOP.Activation.Strategies;
using Ninject.Extensions.AOP.Components;
using Ninject.Extensions.AOP.Modules;
using Ninject.Modules;

namespace Ninject.Extensions.AOP.Tests.Modules
{
    [TestClass]
    public class InterceptionModuleTestFixture
    {
        private NinjectModule interceptionModule;
        private IKernel kernel;

        [TestInitialize]
        public void Initialize()
        {
            this.interceptionModule = new InterceptionModule();
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
            var activationStrategy = this.kernel.Components.GetAll<IActivationStrategy>().OfType<InterceptActivationStrategy>().FirstOrDefault();

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
            var interceptionRegistry = this.kernel.Components.Get<IInterceptionRegistry>();

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
            var interceptionRegistry = this.kernel.Components.Get<IInterceptionRegistry>();

            Assert.IsNotNull(interceptionRegistry);
        }
    }
}