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
using Ninject.Extensions.AspectsWeaver.Selectors;
using Ninject.Syntax;

namespace Ninject.Extensions.AspectsWeaver.Planning.Bindings
{
    public static class ExtensionsForIBindingSyntaxAspectsWeaver
    {
        /// <summary>
        /// Return the interceptor builder.
        /// </summary>
        /// <typeparam name="T">The type of instance to intercept.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns>The fluent syntax.</returns>
        public static IPointCutOrWeaveIntoSyntax Weave<T>(this Syntax.IBindingOnSyntax<T> builder)
        {
           return new WeaverBuilder(builder.Kernel, builder.BindingConfiguration);
        }

        public static IWeaveIntoSyntax WeaveGetProperty<T>(this IBindingOnSyntax<T> builder, Expression<Func<T, object>> propertyGetter)
        {
            var interceptorBuilder = new WeaverBuilder(builder.Kernel, builder.BindingConfiguration);

            var propertySelector = new GetPropertyCutSelector<T>(propertyGetter);

            return interceptorBuilder.PointCuts(propertySelector);
        }
    }
}