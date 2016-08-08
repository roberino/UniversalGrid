using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace UniversalGrid.Geometry
{
    public class Rectangle : ISpatial2D, IEquatable<Rectangle>
    {
        public Rectangle(int x, int y, int width, int height) : this(new Point2D() { X = x, Y = y }, width, height)
        {
        }

        public Rectangle(Point2D topLeft, int width, int height)
        {
            Contract.Assert(width > 0);
            Contract.Assert(height > 0);

            Width = width;
            Height = height;
            TopLeft = topLeft;
        }

        /// <summary>
        /// Gets the width of the rectangle
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height of the rectangle
        /// </summary>
        public int Height { get; private set; }

        public IEnumerable<Point2D> Positions
        {
            get
            {
                return Enumerable.Range(TopLeft.Y, Height).SelectMany(y => Enumerable.Range(TopLeft.X, Width).Select(x => new Point2D() { X = x, Y = y }));
            }
        }

        /// <summary>
        /// Gets the top left coordinate of the rectangle
        /// </summary>
        public Point2D TopLeft { get; protected set; }

        /// <summary>
        /// Gets the bottom right coordinate of the rectangle
        /// </summary>
        public Point2D BottomRight
        {
            get
            {
                return new Point2D()
                {
                    Y = TopLeft.Y + Height - 1,
                    X = TopLeft.X + Width - 1
                };
            }
        }

        /// <summary>
        /// Returns true if this rectangle overlaps the other positions
        /// </summary>
        public bool Overlaps(IEnumerable<Point2D> positions)
        {
            return positions.Any(p => Overlaps(p));
        }

        /// <summary>
        /// Returns true if this rectangle overlaps the point
        /// </summary>
        public bool Overlaps(Point2D position)
        {
            return position.X >= TopLeft.X && position.Y >= TopLeft.Y && position.X <= BottomRight.X && position.Y <= BottomRight.Y;
        }

        /// <summary>
        /// Returns true if this rectangle overlaps the other object
        /// </summary>
        public bool Overlaps(ISpatial2D spatial)
        {
            return spatial.Positions.Any(p => Overlaps(p));
        }

        /// <summary>
        /// Returns true if this rectangle is wholely within the bounds of the other object
        /// </summary>
        public bool IsWithin(ISpatial2D spatial)
        {
            if (spatial is Rectangle)
            {
                var other = (Rectangle)spatial;

                return TopLeft.X >= other.TopLeft.X &&
                    TopLeft.Y >= other.TopLeft.X &&
                    BottomRight.X <= other.BottomRight.X &&
                    BottomRight.Y <= other.BottomRight.Y;
            }

            return !(Positions.Except(spatial.Positions).Any());
        }

        /// <summary>
        /// Moves the rectangle in the direction and magnitude of the vector
        /// </summary>
        public virtual bool Move(Point2D vector)
        {
            TopLeft = TopLeft.Translate(vector);

            return true;
        }

        public virtual bool Equals(Rectangle other)
        {
            if (other == null) return false;

            if (ReferenceEquals(this, other)) return true;

            return (other.TopLeft.Equals(TopLeft) && other.Width == Width && other.Height == Height);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Rectangle);
        }

        public override int GetHashCode()
        {
            return new Tuple<int, int, int, int>(Width, Height, TopLeft.X, TopLeft.Y).GetHashCode();
        }
    }
}