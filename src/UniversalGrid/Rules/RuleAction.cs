using System;
using System.Collections.Generic;
using UniversalGrid.Geometry;

namespace UniversalGrid.Rules
{
    internal class RuleAction<T> : TypedSpatialRule<Action<T, ISpatial2D>>
    {
        public RuleAction(int id, Action<T, ISpatial2D> action, Func<ISpatial2D, IEnumerable<Point2D>, bool> condition) : base(id, action, condition)
        {
        }
    }
}
