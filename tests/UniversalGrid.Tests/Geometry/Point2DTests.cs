using NUnit.Framework;
using UniversalGrid.Geometry;

namespace UniversalGrid.Tests.Geometry
{
    [TestFixture]
    public class Point2DTests
    {
        [Test]
        public void Equals_ReturnsCorrectBehaviour()
        {
            var point1 = new Point2D()
            {
                X = 5,
                Y = 15
            };

            var point2 = new Point2D()
            {
                X = 5,
                Y = 15
            };

            var point3 = new Point2D()
            {
                X = 5,
                Y = 16
            };

            Assert.That(point2.Equals(point1));
            Assert.That(point2.Equals(point3), Is.False);
        }

        [Test]
        public void Rotate_Example1()
        {
            var point1 = new Point2D()
            {
                X = 5,
                Y = 6
            };

            var origin = new Point2D()
            {
                X = 3,
                Y = 3
            };

            var rotated = point1.Rotate(origin);

            Assert.That(rotated.X > 0);
        }
    }
}
