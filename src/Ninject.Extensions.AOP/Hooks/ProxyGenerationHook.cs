using Castle.DynamicProxy;
using System;
using System.Reflection;

namespace Ninject.Extensions.AOP.Hooks
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
