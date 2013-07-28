// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 
using System;
using System.Linq.Expressions;

namespace Ninject.Extensions.AOP.Planning.Bindings
{
    public static class ExtensionsForIBindingSyntax
    {
        /// <summary>
        /// Return the interceptor builder.
        /// </summary>
        /// <typeparam name="T">The type of instance to intercept.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns>The fluent syntax.</returns>
        public static IInterceptorSyntax Intercept<T>(this Ninject.Syntax.IBindingOnSyntax<T> builder)
        {
            return new InterceptorBuilder(builder.Kernel, builder.BindingConfiguration);
        }

        public static IInterceptorSyntax InterceptProperty<T>(this Ninject.Syntax.IBindingOnSyntax<T> builder, Expression<Func<T, object>> propertyGetter)
        {
            return new InterceptorBuilder(builder.Kernel, builder.BindingConfiguration);
        }
    }
}