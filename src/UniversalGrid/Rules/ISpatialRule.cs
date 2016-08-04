using System;
using System.Collections.Generic;
using UniversalGrid.Geometry;

namespace UniversalGrid.Rules
{
    public interface ISpatialRule
    {
        int Id { get; }
        Func<ISpatial2D, IEnumerable<Point2D>, bool> Condition { get; }
    }
}