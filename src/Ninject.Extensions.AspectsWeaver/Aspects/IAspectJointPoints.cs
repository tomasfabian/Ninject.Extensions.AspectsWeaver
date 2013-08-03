// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using Ninject.Extensions.AspectsWeaver.Aspects.Contracts;

namespace Ninject.Extensions.AspectsWeaver.Aspects
{
    public interface IAspectJointPoints
    {
        void OnBefore(IBeforeArgs args);
        void OnSuccess(ISuccessArgs args);
        void OnException(IExceptionArgs args);
        void OnFinally(IFinallyArgs args);
    }
}
