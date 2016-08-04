using System.Collections.Generic;

namespace UniversalGrid.Geometry
{
    public interface ISpatial2D
    {
        Point2D TopLeft { get; }

        IEnumerable<Point2D> Positions { get; }

        bool Move(Point2D vector);

        bool Overlaps(ISpatial2D spatial);

        bool Overlaps(Point2D position);

        bool Overlaps(IEnumerable<Point2D> positions);

        bool IsWithin(ISpatial2D spatial);
    }
}
