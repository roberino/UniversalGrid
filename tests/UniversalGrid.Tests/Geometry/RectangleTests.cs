using NUnit.Framework;
using System.Linq;
using UniversalGrid.Geometry;

namespace UniversalGrid.Tests.Geometry
{
    [TestFixture]
    public class RectangleTests
    {
        [Test]
        public void CreateInstace_InitialisesAsExpected()
        {
            var rect = new Rectangle(0, 0, 2, 2);

            Assert.That(rect.TopLeft.Equals(new Point2D() { X = 0, Y = 0 }));
            Assert.That(rect.BottomRight.Equals(new Point2D() { X = 2, Y = 2 }));
            Assert.That(rect.Width, Is.EqualTo(2));
            Assert.That(rect.Height, Is.EqualTo(2));
            Assert.That(rect.Positions.Count(), Is.EqualTo(4));

            Assert.That(rect.Positions.ElementAt(0), Is.EqualTo(new Point2D() { X = 0, Y = 0 }));
            Assert.That(rect.Positions.ElementAt(1), Is.EqualTo(new Point2D() { X = 1, Y = 0 }));
            Assert.That(rect.Positions.ElementAt(2), Is.EqualTo(new Point2D() { X = 0, Y = 1 }));
            Assert.That(rect.Positions.ElementAt(3), Is.EqualTo(new Point2D() { X = 1, Y = 1 }));
        }
    }
}
