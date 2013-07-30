// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using Ninject.Extensions.AspectsWeaver.Aspects;

namespace Ninject.Extensions.AspectsWeaver.Planning.Bindings
{
    /// <summary>
    /// Used to define a basic aspects veawer syntax builder.
    /// </summary>
    public interface IWeaveIntoSyntax
    {
        /// <summary>
        /// Indicates that the service should be intercepted with the specified aspect type.
        /// </summary>
        /// <typeparam name="TAspect">The aspect type.</typeparam>
        /// <returns>The fluent syntax.</returns>
        IWeaveIntoSyntax Into<TAspect>() where TAspect : IAspect;
    }

    public interface IJointPointsSyntax
    {
        IWeaveIntoSyntax JointPoints(IJointPointSelector selector);
    }
}