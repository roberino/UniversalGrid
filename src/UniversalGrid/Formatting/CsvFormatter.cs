using System;
using System.Collections.Generic;
using System.IO;
using UniversalGrid.Geometry;

namespace UniversalGrid.Formatting
{
    public class CsvFormatter<T> : ITextFormatter<T>
    {
        private readonly TextWriter _output;
        private readonly Func<T, string> _objectFormatter;

        public CsvFormatter(TextWriter output, Func<T, string> objectFormatter, char _delimitter = '\t')
        {
            _output = output;
            _objectFormatter = objectFormatter;
            
            Delimitter = _delimitter;
        }

        public char Delimitter { get; set; }

        public void WriteStartGrid(IGridContainer<T> grid)
        {
        }

        public void WriteEndGrid()
        {
            _output.Flush();
        }

        public void WriteStartRow(int rowIndex)
        {
        }

        public void WriteEndRow()
        {
            _output.WriteLine();
        }

        public void WriteCell(Point2D cellPos, int cellIndex, IEnumerable<ISpatial2DThing<T>> contents)
        {
            if (cellIndex > 0)
                _output.Write(Delimitter);

            foreach (var item in contents)
            {
                WriteItem(item);
            }
        }

        protected virtual void WriteItem(ISpatial2DThing<T> item)
        {
            var s = _objectFormatter.Invoke(item.Data);
            _output.Write((s != null && s.Contains(Delimitter.ToString())) ? "\"" + s + "\"" : s);
        }
    }
}