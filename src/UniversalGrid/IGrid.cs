using System;
using System.Collections.Generic;
using UniversalGrid.Geometry;

namespace UniversalGrid
{
    public interface IGrid : ISpatial2D
    {
        /// <summary>
        /// Gets the rows of the grid
        /// </summary>
        IEnumerable<IEnumerable<Point2D>> Rows { get; }

        /// <summary>
        /// Gets or sets a viewport through which the grid can be rendered
        /// </summary>
        Rectangle ViewPort { get; set; }

        /// <summary>
        /// Gets or sets the width of 1 unit of space within the grid
        /// </summary>
        double UnitWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of 1 unit of space within the grid
        /// </summary>
        double UnitHeight { get; set; }
    }
}