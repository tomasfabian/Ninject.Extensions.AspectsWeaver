using System;

namespace Ninject.Extensions.AOP.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ExcludeFromInterceptionAttribute : Attribute
    {
    }
}
