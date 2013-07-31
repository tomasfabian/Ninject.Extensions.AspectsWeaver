// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using Ninject.Extensions.AspectsWeaver.Aspects;

namespace Ninject.Extensions.AspectsWeaver.Selectors
{
    public class CompositeJointPointSelector : IJointPointSelector
    {
        private readonly IList<IJointPointSelector> selectors;

        public CompositeJointPointSelector(IEnumerable<IJointPointSelector> selectors)
        {
            this.selectors = new List<IJointPointSelector>(selectors);
        }

        public bool IsJointPoint(Type type, MethodInfo method)
        {
            return this.selectors.All(c => c.IsJointPoint(type, method));
        }
    }
}
