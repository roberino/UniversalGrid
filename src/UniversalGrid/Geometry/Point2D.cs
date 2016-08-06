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
            return this + vector;
        }

        /// <summary>
        /// Rotates a point, returning a new point
        /// </summary>
        public Point2D Rotate(int angle = 90)
        {
            return Rotate(new Point2D(), angle);
        }

        /// <summary>
        /// Rotates a point around an origin, returning a new point
        /// </summary>
        public Point2D Rotate(Point2D origin, int angle = 90)
        {
            var s = Math.Sin(angle);
            var c = Math.Cos(angle);

            //p'x = cos(theta) * (px-ox) - sin(theta) * (py-oy) + ox
            //p'y = sin(theta) * (px-ox) + cos(theta) * (py-oy) + oy

            var n = new Point2D
            {
                X = Round((c * (X - origin.X) - s * (Y - origin.Y) + origin.X)),
                Y = Round((s * (X - origin.X) - c * (Y - origin.Y) + origin.Y)),
            };

            return n;
        }

        private int Round(double x)
        {
            if(RoundingMethod == RoundingMethod.Truncate)
                return (int)Math.Truncate(x);

            return (int)Math.Round(x, 0);
        }

        public static RoundingMethod RoundingMethod { get; set; } = RoundingMethod.Truncate;

        public static Point2D operator +(Point2D p1, Point2D p2)
        {
            return new Point2D()
            {
                X = p1.X + p2.X,
                Y = p1.Y + p2.Y
            };
        }

        public static Point2D operator -(Point2D p1, Point2D p2)
        {
            return new Point2D()
            {
                X = p1.X - p2.X,
                Y = p1.Y - p2.Y
            };
        }

        public static Point2D operator *(Point2D p1, Point2D p2)
        {
            return new Point2D()
            {
                X = p1.X * p2.X,
                Y = p1.Y * p2.Y
            };
        }

        public static Point2D operator /(Point2D p1, Point2D p2)
        {
            return new Point2D()
            {
                X = p1.X / p2.X,
                Y = p1.Y / p2.Y
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