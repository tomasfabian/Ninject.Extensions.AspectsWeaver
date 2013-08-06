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

namespace Ninject.Extensions.AspectsWeaver.Syntax
{
    public static class ExtensionsForIBindingSyntaxAspectsWeaver
    {
        /// <summary>
        /// Returns the weaver bindingSyntax's fluent syntax.
        /// </summary>
        /// <typeparam name="T">The type of instance to intercept.</typeparam>
        /// <param name="bindingSyntax">The bindingSyntax.</param>
        /// <returns>The fluent syntax.</returns>
        public static IPointCutOrWeaveIntoSyntax Weave<T>(this IBindingOnSyntax<T> bindingSyntax)
        {
           return new WeaverBuilder(bindingSyntax.Kernel, bindingSyntax.BindingConfiguration);
        }

        /// <summary>
        /// Adds a property getter pointcut and returns the weaver bindingSyntax's fluent syntax.
        /// </summary>
        /// <typeparam name="T">The type of instance to intercept.</typeparam>
        /// <param name="bindingSyntax">The bindingSyntax.</param>
        /// <param name="propertyGetter">Expression to get the property to intercept.</param>
        /// <returns>The fluent syntax for adding aspects.</returns>
        public static IWeaveIntoSyntax WeaveGetProperty<T>(this IBindingOnSyntax<T> bindingSyntax, Expression<Func<T, object>> propertyGetter)
        {
            var interceptorBuilder = new WeaverBuilder(bindingSyntax.Kernel, bindingSyntax.BindingConfiguration);

            var propertySelector = new GetPropertyPointCutSelector<T>(propertyGetter);

            return interceptorBuilder.PointCuts(propertySelector);
        }

        /// <summary>
        /// Adds a property setter pointcut and returns the weaver bindingSyntax's fluent syntax.
        /// </summary>
        /// <typeparam name="T">The type of instance to intercept.</typeparam>
        /// <param name="bindingSyntax">The binding syntax.</param>
        /// <param name="propertyGetter">Expression to get the property to intercept.</param>
        /// <returns>The fluent syntax for adding aspects.</returns>
        public static IWeaveIntoSyntax WeaveSetProperty<T>(this IBindingOnSyntax<T> bindingSyntax, Expression<Func<T, object>> propertyGetter)
        {
            var interceptorBuilder = new WeaverBuilder(bindingSyntax.Kernel, bindingSyntax.BindingConfiguration);

            var propertySelector = new SetPropertyPointCutSelector<T>(propertyGetter);

            return interceptorBuilder.PointCuts(propertySelector);
        }
    }
}