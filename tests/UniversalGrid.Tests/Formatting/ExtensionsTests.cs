using NUnit.Framework;
using System;
using System.Linq;
using System.Xml.Linq;
using UniversalGrid.Formatting;

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
        public void ToHtml_ReturnsValidXhtmlTable()
        {
            var grid = new UniversalGrid<string>(10, 10);

            grid.SetObject("X", 5, 5);

            var html = grid.ToHtml("tbl");

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
