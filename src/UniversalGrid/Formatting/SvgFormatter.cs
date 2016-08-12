using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using UniversalGrid.Drawing;
using UniversalGrid.Geometry;

namespace UniversalGrid.Formatting
{
    public class SvgFormatter<T> : XmlFormatter<T>
    {
        private readonly string _className;

        private double _unitWidth;
        private double _unitHeight;

        public SvgFormatter(XmlWriter output, Func<T, XNode> objectFormatter, string className = null) : base(output, objectFormatter)
        {
            _className = className;

            _unitWidth = 50;
            _unitHeight = 50;
            BorderStroke = Colour.Black;
        }
        
        public Colour BorderStroke { get; set; }
        public Colour CellFill { get; set; }

        protected override void WriteStartRootDocument(IGridContainer<T> grid)
        {
            _unitWidth = grid.UnitWidth;
            _unitHeight = grid.UnitHeight;

            XmlWriter.WriteStartElement("svg", "http://www.w3.org/2000/svg");

            if (!string.IsNullOrEmpty(_className))
            {
                XmlWriter.WriteAttributeString("class", _className);
            }

            XmlWriter.WriteAttributeString("width", (_unitWidth * grid.Width).ToString());
            XmlWriter.WriteAttributeString("height", (_unitHeight * grid.Height).ToString());
        }

        public override void WriteStartRow(int rowIndex)
        {
            WriteStartElement("g");
            XmlWriter.WriteAttributeString("class", "row");
        }

        protected void WriteItem(Point2D cellPos, ISpatial2DThing<T> item)
        {
            if (item.TopLeft.Equals(cellPos))
            {
                WriteStartElement("g");

                var itemNode = ObjectFormatter.Invoke(item.Data);

                foreach (var pos in item.Positions)
                {
                    WriteRect(pos, item.Colour, new Colour());
                }

                if (itemNode != null)
                {
                    var cent = item.RotationalCentre;

                    WriteStartElement("text");

                    XmlWriter.WriteAttributeString("x", ((cent.X + 0.5) * _unitWidth).ToString());
                    XmlWriter.WriteAttributeString("y", ((cent.Y + 0.5) * _unitHeight).ToString());

                    itemNode.WriteTo(XmlWriter);

                    XmlWriter.WriteEndElement();
                }

                XmlWriter.WriteEndElement();
            }
        }

        public override void WriteCell(Point2D cellPos, int cellIndex, IEnumerable<ISpatial2DThing<T>> contents)
        {
            XmlWriter.WriteStartElement("g");
            XmlWriter.WriteAttributeString("class", "cell");

            WriteRect(cellPos, CellFill, BorderStroke);
            
            foreach (var item in contents)
            {
                WriteItem(cellPos, item);
            }

            XmlWriter.WriteEndElement();
        }

        private void WriteRect(Point2D pos, Colour fill, Colour? stroke = null)
        {
            XmlWriter.WriteStartElement("rect");

            //x="10" y="10" width="30" height="30" stroke="black"

            XmlWriter.WriteAttributeString("x", (pos.X * _unitWidth).ToString());
            XmlWriter.WriteAttributeString("y", (pos.Y * _unitHeight).ToString());
            XmlWriter.WriteAttributeString("width", _unitWidth.ToString());
            XmlWriter.WriteAttributeString("height", _unitHeight.ToString());

            if (stroke.HasValue)
            {
                XmlWriter.WriteAttributeString("stroke", ColourStr(stroke.Value));
                XmlWriter.WriteAttributeString("stroke-width", "1");
            }

            XmlWriter.WriteAttributeString("fill", ColourStr(fill));

            XmlWriter.WriteEndElement();
        }

        private string ColourStr(Colour colour)
        {
            if (colour.IsTransparent) return "transparent";

            return colour.ToHex();
        }
    }
}