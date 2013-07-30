// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using Castle.DynamicProxy;
using System;
using System.Reflection;

namespace Ninject.Extensions.AspectsWeaver.Hooks
{
    public abstract class ProxyGenerationHook : IProxyGenerationHook
    {
        public bool ShouldInterceptMethod(Type type, MethodInfo memberInfo)
        {
            return this.OnShouldInterceptMethod(type, memberInfo);
        }

        protected abstract bool OnShouldInterceptMethod(Type type, MethodInfo memberInfo);

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            this.OnNonProxyableMemberNotification(type, memberInfo);
        }

        protected virtual void OnNonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
        }

        public void MethodsInspected()
        {
            this.OnMethodsInspected();
        }

        protected virtual void OnMethodsInspected()
        {
        }
    }
}
