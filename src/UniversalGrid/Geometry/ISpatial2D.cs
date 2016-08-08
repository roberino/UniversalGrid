using System.Collections.Generic;

namespace UniversalGrid.Geometry
{
    public interface ISpatial2D
    {
        /// <summary>
        /// Returns the top left coordinate
        /// </summary>
        Point2D TopLeft { get; }

        /// <summary>
        /// Returns all coordinates occupied by the object
        /// </summary>
        IEnumerable<Point2D> Positions { get; }

        /// <summary>
        /// Moves the object if possible by translation via the vector
        /// </summary>
        /// <param name="vector">A point specifying direction and magnitude</param>
        /// <returns>True if the object was moved</returns>
        bool Move(Point2D vector);

        /// <summary>
        /// Returns true if this object overlaps the other object
        /// </summary>
        /// <param name="spatial">An other spatial object</param>
        bool Overlaps(ISpatial2D spatial);

        /// <summary>
        /// Returns true if this object overlaps the point
        /// </summary>
        bool Overlaps(Point2D position);

        /// <summary>
        /// Returns true if this object overlaps any of the points
        /// </summary>
        bool Overlaps(IEnumerable<Point2D> positions);

        /// <summary>
        /// Returns true if this object is wholely within the bounds of the other object
        /// </summary>
        bool IsWithin(ISpatial2D spatial);
    }
}
