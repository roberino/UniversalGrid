using System.Collections.Generic;
using UniversalGrid.Geometry;

namespace UniversalGrid.Formatting
{
    public interface ITextFormatter<T>
    {
        void WriteStartGrid();
        void WriteEndGrid();
        void WriteStartRow(int rowIndex);
        void WriteEndRow();
        void WriteEmptyCell(int cellIndex);
        void WriteCell(int cellIndex, IEnumerable<ISpatial2DThing<T>> contents);
    }
}