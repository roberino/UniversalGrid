using System.Collections.Generic;
using UniversalGrid.Geometry;

namespace UniversalGrid.Formatting
{
    public interface ITextFormatter<T>
    {
        void WriteStartGrid(IGridContainer<T> grid);
        void WriteEndGrid();
        void WriteStartRow(int rowIndex);
        void WriteEndRow();
        void WriteCell(Point2D cellPos, int cellIndex, IEnumerable<ISpatial2DThing<T>> contents);
    }
}