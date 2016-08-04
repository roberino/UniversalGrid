using System.Collections.Generic;

namespace UniversalGrid.Geometry
{
    public class Point2DEqualityComparer : IEqualityComparer<Point2D>
    {
        private static readonly Point2DEqualityComparer _instance = new Point2DEqualityComparer();

        private Point2DEqualityComparer() { }

        public static Point2DEqualityComparer Comparer
        {
            get
            {
                return _instance;
            }
        }

        public bool Equals(Point2D x, Point2D y)
        {
            return x.X == y.X && x.Y == y.Y;
        }

        public int GetHashCode(Point2D obj)
        {
            return obj.GetHashCode();
        }
    }
}
