// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Moq;
using Ninject.Extensions.AspectsWeaver.Aspects;
using Ninject.Extensions.AspectsWeaver.Aspects.Contracts;

namespace Ninject.Extensions.AspectsWeaver.Tests.Fakes
{
    public class FakeAspect2 : FakeAspect {}

    public class FakeAspect : IAspect
    {
        public FakeAspect()
        {
            this.Setup();
        }

        private readonly Mock<IInterceptor> interceptor = new Mock<IInterceptor>();

        public Mock<IInterceptor> Interceptor
        {
            get { return this.interceptor; }
        }

        public void Intercept(IInvocation invocation)
        {
            interceptor.Object.Intercept(invocation);

            invocation.Proceed();

            if (GetReturnValue != null)
            {
                invocation.ReturnValue = GetReturnValue();
            }
        }

        public Func<object> GetReturnValue;
        
        private int calledTimes;
        public int CalledTimes
        {
            get { return this.calledTimes; }
        }


        private readonly IList<string> calledMethods = new List<string>();
                
        public  IList<string> CalledMethods
        {
            get { return this.calledMethods; }
        }

        private void Setup()
        {
            this.Interceptor.Setup(c => c.Intercept(It.IsAny<IInvocation>()))
                .Callback<IInvocation>(i =>
                                           {
                                               calledMethods.Add(i.Method.Name);
                                               calledTimes++;
                                           });
        }

        public void Reset()
        {
            this.calledTimes = 0;
        }

        public void OnBefore(IBeforeArgs args)
        {
        }

        public void OnSuccess(ISuccessArgs args)
        {
        }

        public void OnException(IExceptionArgs args)
        {
        }

        public void OnFinally(IFinallyArgs args)
        {
        }
    }
}