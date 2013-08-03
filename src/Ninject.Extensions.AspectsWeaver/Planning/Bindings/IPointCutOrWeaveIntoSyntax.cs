// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 
namespace Ninject.Extensions.AspectsWeaver.Planning.Bindings
{
    /// <summary>
    /// Fluent syntax for weave into or pointcut syntax continuation.
    /// </summary>
    public interface IPointCutOrWeaveIntoSyntax : IWeaveIntoSyntax, IPointCutSyntax
    {
    }
}