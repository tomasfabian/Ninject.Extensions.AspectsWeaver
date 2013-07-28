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

                invocation.ReturnValue = this.OnSuccess(invocation.ReturnValue);
            }
            catch (Exception error)
            {
                this.OnException(error);
            }
            finally
            {
                this.OnExit();
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

        protected virtual object OnSuccess(object returnValue)
        {
            return returnValue;
        }

        protected virtual void OnException(Exception error)
        {
        }

        protected virtual void OnExit()
        {
        }

        public static IInterceptor[] EmptyInterceptors = new IInterceptor[] { };
    }
}
