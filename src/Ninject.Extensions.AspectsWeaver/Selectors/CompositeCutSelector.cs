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
    internal class CompositeCutSelector : IPointCutSelector
    {
        private readonly IList<IPointCutSelector> selectors;

        public CompositeCutSelector(IEnumerable<IPointCutSelector> selectors)
        {
            this.selectors = new List<IPointCutSelector>(selectors);
        }

        public bool IsPointCut(Type type, MethodInfo method)
        {
            return this.selectors.All(c => c.IsPointCut(type, method));
        }
    }
}
