using System;
using UniversalGrid.Geometry;

namespace UniversalGrid.Rules
{
    public class RuleViolationEvent : EventArgs
    {
        public RuleViolationEvent(ISpatialRule rule, ISpatial2D target)
        {
            Rule = rule;
            Target = target;
        }

        public ISpatialRule Rule { get; private set; }

        public ISpatial2D Target { get; private set; }
    }
}
