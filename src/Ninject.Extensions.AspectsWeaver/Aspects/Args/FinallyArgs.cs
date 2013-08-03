// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using Castle.DynamicProxy;
using Ninject.Extensions.AspectsWeaver.Aspects.Contracts;

namespace Ninject.Extensions.AspectsWeaver.Aspects.Args
{
    public class FinallyArgs : InvocationArgs, IFinallyArgs
    {
        public FinallyArgs(IInvocation invocation) : base(invocation)
        {
        }
    }
}