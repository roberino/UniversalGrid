using System;

namespace UniversalGrid.Geometry
{
    public class Point2DEventArgs : EventArgs
    {
        public Point2DEventArgs(Point2D point)
        {
            Point = point;
        }

        public Point2D Point { get; private set; }
    }
}
