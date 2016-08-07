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
        /// If set, the point floats between the x integral value. e.g. 2 becomes 2.5
        /// </summary>
        public bool OffsetX { get; set; }

        /// <summary>
        /// If set, the point floats between the y integral value. e.g. 2 becomes 2.5
        /// </summary>
        public bool OffsetY { get; set; }

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
            return other.X == X && other.Y == Y && other.OffsetX == OffsetX && other.OffsetY == OffsetY;
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
            if (RoundingMethod == RoundingMethod.Default)
            {
                if (angle == 90 || angle == -270)
                {
                    var t = this - origin;
                    return new Point2D() { X = -t.Y, Y = t.X, OffsetX = OffsetX, OffsetY = OffsetY } + origin;
                }

                if (angle == 180 || angle == -180)
                {
                    var t = this - origin;
                    return new Point2D() { X = -t.X, Y = -t.Y, OffsetX = OffsetX, OffsetY = OffsetY } + origin;
                }

                if (angle == 270 || angle == -90)
                {
                    var t = this - origin;
                    return new Point2D() { X = t.Y, Y = -t.X, OffsetX = OffsetX, OffsetY = OffsetY } + origin;
                }

                if (angle == 360)
                {
                    return this;
                }
            }

            var s = Math.Sin(angle);
            var c = Math.Cos(angle);
            var ox = origin.Xf;
            var oy = origin.Yf;
            var tx = Xf - ox;
            var ty = Yf - oy;

            //p'x = cos(theta) * (px-ox) - sin(theta) * (py-oy) + ox
            //p'y = sin(theta) * (px-ox) + cos(theta) * (py-oy) + oy

            var n = new Point2D
            {
                X = Round(c * tx - s * ty + ox),
                Y = Round(s * tx + c * ty + oy),
                OffsetX = OffsetX,
                OffsetY = OffsetY
            };

            return n;
        }

        internal double Xf
        {
            get
            {
                return X + (OffsetX ? 0.5d : 0d);
            }
        }

        internal double Yf
        {
            get
            {
                return Y + (OffsetY ? 0.5d : 0d);
            }
        }

        private int Round(double x)
        {
            if(RoundingMethod == RoundingMethod.Truncate)
                return (int)Math.Truncate(x);
            if (RoundingMethod == RoundingMethod.TruncateUp)
                return (int)(x + 0.5);

            return (int)Math.Round(x, 0);
        }

        public static RoundingMethod RoundingMethod { get; set; } = RoundingMethod.Default;

        public static Point2D operator +(Point2D p1, Point2D p2)
        {
            return new Point2D()
            {
                X = p1.X + p2.X,
                Y = p1.Y + p2.Y,
                OffsetX = p1.OffsetX ^ p2.OffsetX,
                OffsetY = p1.OffsetY ^ p2.OffsetY
            };
        }

        public static Point2D operator -(Point2D p1, Point2D p2)
        {
            return new Point2D()
            {
                X = p1.X - p2.X,
                Y = p1.Y - p2.Y,
                OffsetX = p1.OffsetX ^ p2.OffsetX,
                OffsetY = p1.OffsetY ^ p2.OffsetY
            };
        }

        public static Point2D operator *(Point2D p1, Point2D p2)
        {
            return new Point2D()
            {
                X = p1.X * p2.X,
                Y = p1.Y * p2.Y,
                OffsetX = p1.OffsetX ^ p2.OffsetX,
                OffsetY = p1.OffsetY ^ p2.OffsetY
            };
        }

        public static Point2D operator /(Point2D p1, Point2D p2)
        {
            return new Point2D()
            {
                X = p1.X / p2.X,
                Y = p1.Y / p2.Y,
                OffsetX = p1.OffsetX ^ p2.OffsetX,
                OffsetY = p1.OffsetY ^ p2.OffsetY
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