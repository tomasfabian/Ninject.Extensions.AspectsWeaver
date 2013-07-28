using Castle.DynamicProxy;

namespace Ninject.Extensions.AOP.Planning.Bindings
{
    public interface IInterceptorSelectorSyntax
    {
        void FilterInterceptionWith(IInterceptorSelector selector);
    }
}
