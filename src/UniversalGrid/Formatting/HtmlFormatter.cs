using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UniversalGrid.Geometry;

namespace UniversalGrid.Formatting
{
    public class HtmlFormatter<T> : XmlFormatter<T>
    {
        private readonly string _className;

        public HtmlFormatter(TextWriter output, Func<T, string> objectFormatter, string className = null) : base(output, objectFormatter)
        {
            _className = className;
            EmptyCellContents = ((char)160).ToString();
        }

        protected override void WriteStartRootDocument(IGridContainer<T> grid)
        {
            XmlWriter.WriteStartElement("table");

            if (!string.IsNullOrEmpty(_className))
            {
                XmlWriter.WriteAttributeString("class", _className);
            }
        }

        protected override void WriteStartCell()
        {
            XmlWriter.WriteStartElement("td");
        }

        public override void WriteStartRow(int rowIndex)
        {
            WriteStartElement("tr");
        }

        public override void WriteCell(Point2D cellPos, int cellIndex, IEnumerable<ISpatial2DThing<T>> contents)
        {
            WriteStartCell();

            ISpatial2DThing<T> last = contents.LastOrDefault();

            if (last != null && !last.Colour.IsTransparent)
            {
                XmlWriter.WriteAttributeString("style", "color: " + last.Colour.ToHex());
            }

            if (contents.Count() == 1 && !string.IsNullOrEmpty(last.Id))
            {
                XmlWriter.WriteAttributeString("id", last.Id);
            }

            foreach (var item in contents)
            {
                WriteItem(item);
            }

            XmlWriter.WriteEndElement();
        }
    }
}