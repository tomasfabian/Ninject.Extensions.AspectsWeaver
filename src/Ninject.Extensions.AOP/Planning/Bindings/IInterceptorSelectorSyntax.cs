// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 
using Castle.DynamicProxy;
using Ninject.Extensions.AOP.Selectors;

namespace Ninject.Extensions.AOP.Planning.Bindings
{
    public interface IInterceptorSelectorSyntax
    {
        void AllowInterceptionWith(IAllowInterceptionSelector selector);
        void ExcludeInterceptionWith(IExcludeInterceptionSelector selector);
    }
}
