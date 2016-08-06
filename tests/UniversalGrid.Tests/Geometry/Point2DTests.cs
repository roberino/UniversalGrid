using NUnit.Framework;
using UniversalGrid.Geometry;

namespace UniversalGrid.Tests.Geometry
{
    [TestFixture]
    public class Point2DTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Point2D.RoundingMethod = RoundingMethod.Round;
        }

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

        [TestCase(5, 6, -8, 7)]
        [TestCase(2, 2, -3, 3)]
        public void Rotate_Example1(int x, int y, int x1, int y1)
        {
            var point1 = new Point2D()
            {
                X = x,
                Y = y
            };

            var rotated = point1.Rotate();

            Assert.That(rotated.X, Is.EqualTo(x1));
            Assert.That(rotated.Y, Is.EqualTo(y1));
        }

        [Test]
        public void GetHashCode_ReturnsValidResults()
        {
            var point1 = new Point2D() { X = 1, Y = 1 };
            var point2 = new Point2D();
            var point3 = new Point2D();
            var point4 = new Point2D() { X = 2, Y = 1 };
            var point5 = new Point2D() { X = 2, Y = 1 };

            Assert.That(point1.GetHashCode(), Is.EqualTo(point1.GetHashCode()));
            Assert.That(point2.GetHashCode(), Is.EqualTo(point3.GetHashCode()));
            Assert.That(point4.GetHashCode(), Is.EqualTo(point5.GetHashCode()));
            Assert.That(point1.GetHashCode(), Is.Not.EqualTo(point5.GetHashCode()));
        }
    }
}
