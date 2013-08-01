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
using Ninject.Activation;
using Ninject.Activation.Strategies;
using Castle.DynamicProxy;
using Ninject.Extensions.AspectsWeaver.Components;
using Ninject.Extensions.AspectsWeaver.Selectors;

namespace Ninject.Extensions.AspectsWeaver.Activation.Strategies
{
    /// <summary>
    /// Contributes to a <see cref="IPipeline"/>, and is called during the activation
    /// and deactivation of an instance. Decorates the instance with configured interceptors.
    /// </summary>
    public class AspectsWeaverActivationStrategy : ActivationStrategy
    {
        private readonly ProxyGenerator proxyGenenator = new ProxyGenerator();

        /// <summary>
        /// Contributes to the activation of the instance in the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">A reference to the instance being activated.</param>
        public override void Activate(IContext context, InstanceReference reference)
        {
            var proxyGenerationOptions = new ProxyGenerationOptions();

            var weaverRegistries = context.Kernel.Components.Get<IWeaverRegistry>();

            IList<IInterceptor> allInterceptors = new List<IInterceptor>();

            var interceptorSelectors = new List<SelectorWithItsInterceptors>();

            foreach (var aspectsRegistry in weaverRegistries.GetRegistry(context.Binding.BindingConfiguration))
            {
                var interceptorTypes = aspectsRegistry.GetAspectTypes(context.Binding.BindingConfiguration).ToArray();

                var interceptorSelector = aspectsRegistry.GetSelector(context.Binding.BindingConfiguration);

                
                var interceptors = interceptorTypes.Select(interceptorType => context.Kernel.Get(interceptorType) as IInterceptor).ToArray();
                foreach (var interceptor in interceptors)
                {
                    allInterceptors.Add(interceptor);
                }

                if (interceptorSelector != null)
                {
                    var selector = new SelectorWithItsInterceptors(interceptors, interceptorSelector);
                    interceptorSelectors.Add(selector);
                }
            }

            var finalSelectors = new List<IInterceptorSelector> { new ExcludeJointPointAttributeInterceptorSelector() };
            if (interceptorSelectors.Any())
            {
                var joinableCompositeSelector = new JoinableCompositeSelector(interceptorSelectors);
                finalSelectors.Add(joinableCompositeSelector);
            }

            var compositeSelector = new CompositeInterceptorSelector(finalSelectors);

            proxyGenerationOptions.Selector = compositeSelector;

            if (allInterceptors.Any())
            {
                Func<object, object> getOriginalInstance = null;

                if (context.Request.Service.IsInterface)
                {
                    reference.Instance =
                        proxyGenenator.CreateInterfaceProxyWithTargetInterface(context.Request.Service, reference.Instance, proxyGenerationOptions, allInterceptors.ToArray());

                    getOriginalInstance = GetOriginalInstance;
                }
                else
                {
                    var originalInstance = reference.Instance;

                    reference.Instance =
                        proxyGenenator.CreateClassProxyWithTarget(context.Request.Service, reference.Instance, proxyGenerationOptions, allInterceptors.ToArray());

                    getOriginalInstance = _ => originalInstance;
                }

                RebindDeactivationActionsToOriginalInstance(context, reference.Instance, getOriginalInstance);
            }

            base.Activate(context, reference);
        }

        #region RebindDeactivationActionsToOriginalInstance

        private static void RebindDeactivationActionsToOriginalInstance(IContext context, object instance,
                                                                        Func<object, object> getOriginalInstance)
        {
            var deactivationActions = context.Binding.BindingConfiguration.DeactivationActions.ToArray();

            context.Binding.BindingConfiguration.DeactivationActions.Clear();

            for (int i = 0; i < deactivationActions.Length; i++)
            {
                var deactivationAction = deactivationActions[i];

                Action<IContext, object> fixedDeactivationAction = (IContext c, object inst) =>
                                                                       {
                                                                           var originalInstance =
                                                                               getOriginalInstance(inst);

                                                                           deactivationAction(c, originalInstance);
                                                                       };

                context.Binding.BindingConfiguration.DeactivationActions.Add(fixedDeactivationAction);
            }
        }

        #endregion

        #region GetOriginalInstance

        private static object GetOriginalInstance(object instance)
        {
            var targetAccessor = instance as IProxyTargetAccessor;

            if (targetAccessor != null)
            {
                var originalInstance = targetAccessor.DynProxyGetTarget();

                return originalInstance ?? instance;
            }

            return instance;
        }

        #endregion

        /// <summary>
        /// Contributes to the deactivation of the instance in the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">A reference to the instance being deactivated.</param>
        public override void Deactivate(IContext context, InstanceReference reference)
        {
            base.Deactivate(context, reference);
        }
    }
}
