using System;
using System.Collections.Generic;

namespace UniversalGrid.Geometry
{
    public class Point2DEventArgs : EventArgs
    {
        public Point2DEventArgs(IEnumerable<Point2D> points)
        {
            Points = points;
        }

        public IEnumerable<Point2D> Points { get; private set; }

        public bool Abort { get; set; }
    }
}
