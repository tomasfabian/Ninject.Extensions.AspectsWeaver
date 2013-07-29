// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 
using System;
using Castle.DynamicProxy;

namespace Ninject.Extensions.AOP.Aspects
{

    public abstract class InterceptionAspect : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            this.OnEntry(invocation.Arguments);

            try
            {
                if (!RunInstead)
                {
                    invocation.Proceed();
                }
                
                invocation.ReturnValue = this.OnSuccess(invocation.Arguments, invocation.ReturnValue);
            }
            catch (Exception error)
            {
                this.OnException(invocation.Arguments, error);
            }
            finally
            {
                this.OnExit(invocation.Arguments);
            }
        }

        protected virtual bool RunInstead
        {
            get
            {
                return false;
            }
        }

        protected virtual void OnEntry(object[] arguments)
        {
        }

        protected virtual object OnSuccess(object[] arguments, object returnValue)
        {
            return returnValue;
        }

        protected virtual void OnException(object[] arguments, Exception error)
        {
        }

        protected virtual void OnExit(object[] arguments)
        {
        }

        public static IInterceptor[] EmptyInterceptors = new IInterceptor[] { };
    }
}
