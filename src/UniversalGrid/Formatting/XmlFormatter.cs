using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UniversalGrid.Geometry;

namespace UniversalGrid.Formatting
{
    public class XmlFormatter<T> : ITextFormatter<T>
    {
        private readonly XmlWriter _output;
        private readonly Func<T, XNode> _objectFormatter;

        bool _dataWritten;
        int _currentRow;

        public XmlFormatter(TextWriter output, Func<T, string> objectFormatter)
        {
            _output = XmlWriter.Create(output, new XmlWriterSettings());
            _objectFormatter = (x => x == null ? null : new XText(x.ToString()));
            EmptyCellContents = " ";
        }

        public XmlFormatter(XmlWriter output, Func<T, XNode> objectFormatter)
        {
            _output = output;
            _objectFormatter = objectFormatter;
            EmptyCellContents = " ";
        }

        public string EmptyCellContents { get; set; }

        protected XmlWriter XmlWriter { get { return _output; } }

        protected Func<T, XNode> ObjectFormatter { get { return _objectFormatter; } }

        protected int CurrentRowIndex { get { return _currentRow; } }

        protected virtual void WriteStartElement(string name)
        {
            _output.WriteStartElement(name);
        }

        protected virtual void WriteItem(ISpatial2DThing<T> item)
        {
            var xnode = _objectFormatter.Invoke(item.Data);

            if (xnode != null) xnode.WriteTo(_output);
        }

        protected virtual void WriteStartRootDocument(IGridContainer<T> grid)
        {
            WriteStartElement("grid");

            _output.WriteAttributeString("viewport", grid.ViewPort.ToString());
            _output.WriteAttributeString("width", grid.Width.ToString());
            _output.WriteAttributeString("height", grid.Height.ToString());
        }

        protected virtual void WriteStartCell()
        {
            WriteStartElement("cell");
        }

        public void WriteStartGrid(IGridContainer<T> grid)
        {
            if (!_dataWritten)
            {
                _output.WriteStartDocument();
                _dataWritten = true;
            }

            WriteStartRootDocument(grid);
        }

        public void WriteEndGrid()
        {
            _output.WriteEndElement();
            _output.Flush();
        }

        public virtual void WriteStartRow(int rowIndex)
        {
            _currentRow = rowIndex;
            WriteStartElement("row");
        }

        public void WriteEndRow()
        {
            _output.WriteEndElement();
        }

        public virtual void WriteCell(Point2D cellPos, int cellIndex, IEnumerable<ISpatial2DThing<T>> contents)
        {
            ISpatial2DThing<T> last = contents.LastOrDefault();

            WriteStartCell();

            if (last != null)
            {
                _output.WriteAttributeString("color", last.Colour.ToHex());
                _output.WriteAttributeString("label", last.Label);
                _output.WriteAttributeString("selected", last.Selected.ToString());
            }

            foreach (var item in contents)
            {
                WriteItem(item);
            }

            _output.WriteEndElement();
        }
    }
}