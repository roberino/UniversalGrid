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

        /// <summary>
        /// Gets the relevant points
        /// </summary>
        public IEnumerable<Point2D> Points { get; private set; }

        /// <summary>
        /// Gets or sets a flag which will abort the current operation
        /// </summary>
        public bool Abort { get; set; }
    }
}
