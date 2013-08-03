// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;
using Castle.DynamicProxy;
using Ninject.Extensions.AspectsWeaver.Aspects.Contracts;

namespace Ninject.Extensions.AspectsWeaver.Aspects.Args
{
    public class InvocationArgs : IInvocationArgs
    {
        private readonly IInvocation invocation;

        public InvocationArgs(IInvocation invocation)
        {
            if (invocation == null) throw new ArgumentNullException("invocation");
            this.invocation = invocation;
        }

        public IInvocation Invocation { get { return invocation; } }
    }
}