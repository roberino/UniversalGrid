using NUnit.Framework;
using System;
using System.Linq;
using System.Xml.Linq;
using UniversalGrid.Drawing;
using UniversalGrid.Formatting;
using UniversalGrid.Geometry;

namespace UniversalGrid.Tests.Formatting
{
    [TestFixture]
    public class ExtensionsTests
    {
        [Test]
        public void ToCsv_ReturnsValidData()
        {
            var grid = new UniversalGrid<string>(3, 3);

            grid.SetObject("X", 0, 0);
            grid.SetObject("O", 1, 1);
            grid.SetObject("X", 2, 2);

            var csv = grid.ToCsv(',');

            Console.WriteLine(csv);
        }

        [Test]
        public void ToXml_ReturnsValidXmlDoc()
        {
            var grid = new UniversalGrid<string>(10, 15);

            grid.SetObject("X", 5, 5);

            var xmlDoc = grid.ToXml();

            Assert.That(xmlDoc.Root.Name.LocalName, Is.EqualTo("grid"));
            Assert.That(xmlDoc.Root.Attribute("width").Value, Is.EqualTo("10"));
            Assert.That(xmlDoc.Root.Attribute("height").Value, Is.EqualTo("15"));
            Assert.That(xmlDoc.Root.Attribute("viewport").Value, Is.EqualTo("0 0 9 14"));
            Assert.That(xmlDoc.Root.Elements().ElementAt(5).Elements().ElementAt(5).Value, Is.EqualTo("X"));

            Console.WriteLine(xmlDoc);
        }

        [Test]
        public void ToSvg_ReturnsValidSvgDoc()
        {
            var grid = new UniversalGrid<string>(10, 15)
            {
                UnitHeight = 50,
                UnitWidth = 50
            };

            var obj = grid.SetObject("X", 5, 5);

            obj.Colour = new Colour() { R = 255, A = 255 };
            
            var svgDoc = grid.ToSvg();

            Assert.That(svgDoc.Root.Name.LocalName, Is.EqualTo("svg"));

            svgDoc.Save(@"C:\stash\grid.svg");

            Console.WriteLine(svgDoc);
        }

        [Test]
        public void ToSvg_SetsViewBox()
        {
            var grid = new UniversalGrid<string>(10, 15)
            {
                UnitHeight = 50,
                UnitWidth = 50
            };

            var obj = grid.SetObject("X", 5, 5);

            obj.Colour = new Colour() { R = 255, A = 255 };

            var svgDoc = grid.ToSvg(new Rectangle(5, 4, 10, 15), "x", f => new XElement("circ", new XAttribute("cx", f.TopLeft.X)));

            Assert.That(svgDoc.Root.Name.LocalName, Is.EqualTo("svg"));

            Assert.That(svgDoc.Root.Attribute("viewBox").Value, Is.EqualTo("5 4 10 15"));
            Assert.That(svgDoc.Root.Attribute("width").Value, Is.EqualTo("10"));
            Assert.That(svgDoc.Root.Attribute("height").Value, Is.EqualTo("15"));

            Console.WriteLine(svgDoc);
        }

        [Test]
        public void ToHtml_ReturnsValidXhtmlTable()
        {
            var grid = new UniversalGrid<string>(10, 10);

            grid.SetObject("X", 5, 5);

            var html = grid.ToHtml(tableClass: "tbl");

            var htmlDoc = XDocument.Parse(html);

            Assert.That(htmlDoc.Root.Name.LocalName, Is.EqualTo("table"));
            Assert.That(htmlDoc.Root.Attribute("class").Value, Is.EqualTo("tbl"));
            Assert.That(htmlDoc.Root.Elements().ElementAt(5).Elements().ElementAt(5).Value, Is.EqualTo("X"));
            
            Console.WriteLine(htmlDoc);
        }

        [Test]
        public void ToHtml_SetsColour()
        {
            var grid = new UniversalGrid<string>(10, 10);

            var obj = grid.SetObject("X", 5, 5);

            obj.Colour = new Drawing.Colour()
            {
                R = 255,
                A = 255
            };

            var html = grid.ToHtml();

            var htmlDoc = XDocument.Parse(html);

            Assert.That(htmlDoc.Root.Elements().ElementAt(5).Elements().ElementAt(5).Attribute("style").Value, Is.EqualTo("color: #ff0000"));

            Console.WriteLine(htmlDoc);
        }
    }
}
