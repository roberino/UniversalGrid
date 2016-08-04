using System;
using System.Collections.Generic;
using UniversalGrid.Geometry;

namespace UniversalGrid.Rules
{
    public class TypedSpatialRule<T> : ISpatialRule
    {
        public TypedSpatialRule(int id, T type, Func<ISpatial2D, IEnumerable<Point2D>, bool> condition)
        {
            Id = id;
            RuleType = type;
            Condition = condition;
        }

        public Func<ISpatial2D, IEnumerable<Point2D>, bool> Condition { get; private set; }

        public int Id { get; private set; }

        public T RuleType { get; private set; }
    }
}
