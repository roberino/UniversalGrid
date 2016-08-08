using System;
using System.Collections.Generic;
using UniversalGrid.Geometry;

namespace UniversalGrid.Rules
{
    /// <summary>
    /// Represents a constraint
    /// </summary>
    public interface ISpatialRule // TODO: Rename to ISpatialConstraint ? 
    {
        /// <summary>
        /// Gets the id of the rule
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the condition that sets of the rule
        /// </summary>
        Func<ISpatial2D, IEnumerable<Point2D>, bool> Condition { get; }
    }
}