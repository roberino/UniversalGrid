using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversalGrid.Geometry
{
    public class Rectangle : ISpatial2D
    {
        public Rectangle(Point2D topLeft, int width, int height)
        {
            Width = width;
            Height = height;
            TopLeft = topLeft;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public IEnumerable<Point2D> Positions
        {
            get
            {
                var br = BottomRight;
                return Enumerable.Range(TopLeft.Y, br.Y).SelectMany(y => Enumerable.Range(TopLeft.X, br.X).Select(x => new Point2D() { X = x, Y = y }));
            }
        }

        public Point2D TopLeft { get; protected set; }

        public Point2D BottomRight
        {
            get
            {
                return new Point2D()
                {
                    Y = TopLeft.Y + Height,
                    X = TopLeft.X + Width
                };
            }
        }

        public bool Overlaps(Point2D position)
        {
            return position.X >= TopLeft.X && position.Y >= TopLeft.Y && position.X <= BottomRight.X && position.Y <= BottomRight.Y;
        }

        public bool Overlaps(ISpatial2D spatial)
        {
            return spatial.Positions.Any(p => Overlaps(p));
        }

        public bool IsWithin(ISpatial2D spatial)
        {
            return spatial.Positions.All(p => Overlaps(p));
        }

        public void Move(Point2D vector)
        {
            TopLeft = TopLeft.Translate(vector);
        }
    }
}