using System.Collections.Generic;

namespace UniversalGrid.Geometry
{
    public interface ISpatial2D
    {
        Point2D TopLeft { get; }

        IEnumerable<Point2D> Positions { get; }

        void Move(Point2D vector);

        bool Overlaps(ISpatial2D spatial);

        bool Overlaps(Point2D position);

        bool IsWithin(ISpatial2D spatial);
    }
}
