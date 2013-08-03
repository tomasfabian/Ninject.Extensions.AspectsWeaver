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
    public class ExceptionArgs : InvocationArgs, IExceptionArgs
    {
        private readonly Exception error;

        public ExceptionArgs(IInvocation invocation, Exception error)
            : base(invocation)
        {
            if (error == null) throw new ArgumentNullException("error");
            this.error = error;
        }

        public Exception Error
        {
            get { return error; }
        }
    }
}