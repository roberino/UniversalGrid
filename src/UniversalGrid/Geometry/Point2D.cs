using System;

namespace UniversalGrid.Geometry
{
    /// <summary>
    /// Returns a integral point in two dimensional space
    /// </summary>
    public struct Point2D : IEquatable<Point2D>, IComparable<Point2D>
    {
        /// <summary>
        /// The X-axis value
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// The Y-Axis value
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Compares this point with another, with the Y axis taking precedence
        /// </summary>
        public int CompareTo(Point2D other)
        {
            var yc = other.Y.CompareTo(Y);

            if (yc != 0) return yc;

            return other.X.CompareTo(X);
        }

        /// <summary>
        /// Returns true if two points occupy the same coordinates
        /// </summary>
        public bool Equals(Point2D other)
        {
            return other.X == X && other.Y == Y;
        }

        /// <summary>
        /// Translates the point using the supplied vector and returning a new point
        /// </summary>
        /// <param name="vector">A vector specifying the X and Y direction</param>
        public Point2D Translate(Point2D vector)
        {
            return new Point2D()
            {
                 X = X + vector.X,
                 Y = Y + vector.Y
            };
        }

        /// <summary>
        /// Rotates a point around an origin, returning a new point
        /// </summary>
        public Point2D Rotate(Point2D origin, int angle = 90)
        {
            var s = Math.Sin(angle);
            var c = Math.Cos(angle);

            return new Point2D
            {
                X = (int)(origin.X * c - origin.Y * s),
                Y = (int)(origin.X * s + origin.Y * c)
            };
        }

        public override bool Equals(object obj)
        {
            if(obj is Point2D)
                return Equals((Point2D)obj);

            return false;
        }

        public override int GetHashCode()
        {
            return new Tuple<int, int>(X, Y).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("(x{0},y{1})", X, Y);
        }
    }
}