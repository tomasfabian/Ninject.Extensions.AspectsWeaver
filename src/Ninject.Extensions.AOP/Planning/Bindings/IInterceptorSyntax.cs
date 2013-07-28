// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 
namespace Ninject.Extensions.AOP.Planning.Bindings
{
    /// <summary>
    /// Used to define a basic interceptor syntax builder.
    /// </summary>
    public interface IInterceptorSyntax
    {
        /// <summary>
        /// Indicates that the service should be intercepted with the specified interceptor type.
        /// </summary>
        /// <typeparam name="TInterceptor">The interceptor type.</typeparam>
        /// <returns>The fluent syntax.</returns>
        IInterceptorOrSelectorSyntax With<TInterceptor>() where TInterceptor : Castle.DynamicProxy.IInterceptor;
    }
}