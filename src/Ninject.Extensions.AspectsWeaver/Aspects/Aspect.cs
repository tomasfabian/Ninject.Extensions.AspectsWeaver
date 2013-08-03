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
using Ninject.Extensions.AspectsWeaver.Aspects.Args;
using Ninject.Extensions.AspectsWeaver.Aspects.Contracts;

namespace Ninject.Extensions.AspectsWeaver.Aspects
{
    public abstract class Aspect : IAspect, IAspectJointPoints
    {
        public void Intercept(IInvocation invocation)
        {
            this.Before(new BeforeArgs(invocation));

            try
            {
                if (!RunInstead)
                {
                    invocation.Proceed();
                }

                this.Success(new SuccessArgs(invocation));
            }
            catch (Exception error)
            {
                this.Exception(new ExceptionArgs(invocation, error));
            }
            finally
            {
                this.Finally(new FinallyArgs(invocation));
            }
        }

        protected virtual bool RunInstead
        {
            get
            {
                return false;
            }
        }

        public void OnBefore(IBeforeArgs args)
        {
            this.Before(args);
        }

        protected virtual void Before(IBeforeArgs args)
        {
        }
        
        public void OnSuccess(ISuccessArgs args)
        {
            this.Success(args);
        }

        protected virtual void Success(ISuccessArgs args)
        {
        }

        
        public void OnException(IExceptionArgs args)
        {
            this.Exception(args);
        }

        protected virtual void Exception(IExceptionArgs args)
        {
        }
        
        public void OnFinally(IFinallyArgs args)
        {
            this.Finally(args);
        }

        protected virtual void Finally(IFinallyArgs args)
        {
        }

        public static IInterceptor[] EmptyInterceptors = new IInterceptor[] { };
    }
}
