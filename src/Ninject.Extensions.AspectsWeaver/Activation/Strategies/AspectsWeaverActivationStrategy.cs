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
using Ninject.Extensions.AspectsWeaver.Aspects;
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

            var interceptionRegistry = context.Kernel.Components.Get<IAspectsRegistry>();
            var interceptorTypes = interceptionRegistry.GetAspectTypes(context.Binding.BindingConfiguration).ToArray();

            if (interceptorTypes.Any())
            {
                var interceptorSelector = interceptionRegistry.GetSelector(context.Binding.BindingConfiguration);

                var jointPointSelectors = new List<IJointPointSelector> { new ExcludeJointPointAttributeSelector() };

                if (interceptorSelector != null)
                {
                    jointPointSelectors.Add(interceptorSelector);
                }

                var compositeSelector = new CompositeJointPointSelector(jointPointSelectors);

                proxyGenerationOptions.Selector = new JointPointSelector(compositeSelector);

                IList<IInterceptor> interceptors = interceptorTypes.Select(interceptorType => context.Kernel.Get(interceptorType) as IInterceptor).ToList();

                Func<object, object> getOriginalInstance = null;

                if (context.Request.Service.IsInterface)
                {
                    reference.Instance =
                        proxyGenenator.CreateInterfaceProxyWithTargetInterface(context.Request.Service, reference.Instance, proxyGenerationOptions, interceptors.ToArray());

                    getOriginalInstance = GetOriginalInstance;
                }
                else
                {
                    var originalInstance = reference.Instance;

                    reference.Instance =
                        proxyGenenator.CreateClassProxyWithTarget(context.Request.Service, reference.Instance, proxyGenerationOptions, interceptors.ToArray());

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
