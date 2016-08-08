using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UniversalGrid.Geometry;

namespace UniversalGrid.Formatting
{
    public class HtmlFormatter<T> : ITextFormatter<T>
    {
        private readonly XmlWriter _output;
        private readonly Func<T, string> _objectFormatter;
        private readonly string _className;

        bool _dataWritten;

        public HtmlFormatter(TextWriter output, Func<T, string> objectFormatter, string className = null)
        {
            _output = XmlWriter.Create(output, new XmlWriterSettings());
            _objectFormatter = objectFormatter;
            _className = className;
            EmptyCellContents = ((char)160).ToString();
        }

        public string EmptyCellContents { get; set; }

        protected XmlWriter XmlWriter { get { return _output; } }

        protected Func<T, string> ObjectFormatter { get { return _objectFormatter; } }

        protected virtual void WriteStartElement(string name)
        {
            _output.WriteStartElement(name);
        }

        protected virtual void WriteItem(ISpatial2DThing<T> item)
        {
            _output.WriteString(_objectFormatter.Invoke(item.Data));
        }

        public void WriteStartGrid()
        {
            if (!_dataWritten)
            {
                _output.WriteStartDocument();
                _dataWritten = true;
            }

            WriteStartElement("table");

            if (!string.IsNullOrEmpty(_className))
            {
                _output.WriteAttributeString("class", _className);
            }
        }

        public void WriteEndGrid()
        {
            _output.WriteEndElement();
            _output.Flush();
        }

        public void WriteStartRow(int rowIndex)
        {
            WriteStartElement("tr");
        }

        public void WriteEndRow()
        {
            _output.WriteEndElement();
        }

        public virtual void WriteEmptyCell(int cellIndex)
        {
            WriteStartElement("td");
            _output.WriteString(EmptyCellContents);
            _output.WriteEndElement();
        }

        public virtual void WriteCell(int cellIndex, IEnumerable<ISpatial2DThing<T>> contents)
        {
            WriteStartElement("td");

            ISpatial2DThing<T> last = contents.LastOrDefault();

            if (last != null && !last.Colour.IsTransparent)
            {
                _output.WriteAttributeString("style", "color: " + last.Colour.ToHex());
            }

            foreach (var item in contents)
            {
                WriteItem(item);
            }

            _output.WriteEndElement();
        }
    }
}