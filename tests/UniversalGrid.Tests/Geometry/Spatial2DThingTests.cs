﻿using NUnit.Framework;
using UniversalGrid.Geometry;

namespace UniversalGrid.Tests.Geometry
{
    [TestFixture]
    public class Spatial2DThingTests
    {
        [Test]
        public void IsWithin_Rectange_ReturnsTrueFor1x1ObjectWithinRect()
        {
            var rect = new Rectangle(0, 0, 5, 5);

            var thing = "A".AsSpatialObject(2, 2);

            Assert.That(thing.IsWithin(rect));
        }

        [Test]
        public void IsWithin_Rectange_ReturnsFalseFor1x1ObjectOutsideRect()
        {
            var rect = new Rectangle(0, 0, 5, 5);

            var thing = "A".AsSpatialObject(20, 2);

            Assert.That(thing.IsWithin(rect), Is.False);
        }

        [Test]
        public void IsWithin_Rectange_ReturnsFalseFor1x2ObjectPartiallyOutsideRect()
        {
            var rect = new Rectangle(0, 0, 5, 5);

            var thing = "A".AsSpatialObject(4, 2, new Point2D() { X = 6, Y = 6 });

            Assert.That(thing.IsWithin(rect), Is.False);
            Assert.That(thing.Overlaps(rect), Is.True);
        }

        [Test]
        public void Equals_ReturnsTrueForObjectsOfEquivPositionAndData()
        {
            var thing1 = new Spatial2DThing<string>(new[] { new Point2D() { X = 1, Y = 2 } });
            var thing2 = new Spatial2DThing<string>(new[] { new Point2D() { X = 1, Y = 2 } });
            var thing3 = new Spatial2DThing<string>(new[] { new Point2D() { X = 1, Y = 3 } });
            var thing4 = new Spatial2DThing<string>(new[] { new Point2D() { X = 1, Y = 2 } });

            thing1.Data = "A";
            thing2.Data = "A";
            thing3.Data = "A";
            thing4.Data = "B";

            Assert.That(thing1.Equals(thing2));
            Assert.That(thing1.Equals(thing1));
            Assert.That(thing1.Equals(thing3), Is.False);
            Assert.That(thing1.Equals(thing4), Is.False);
        }

        [Test]
        public void GetHashCode_ReturnsDifferentValuesForDifferentData()
        {
            var thing1 = new Spatial2DThing<string>(new[] { new Point2D() { X = 1, Y = 2 } });
            var thing2 = new Spatial2DThing<string>(new[] { new Point2D() { X = 1, Y = 2 } });

            thing1.Data = "A";
            thing2.Data = "B";

            Assert.That(thing1.GetHashCode() != thing2.GetHashCode());
        }

        [Test]
        public void GetHashCode_ReturnsDifferentValuesForDifferentPositions()
        {
            var thing1 = new Spatial2DThing<string>(new[] { new Point2D() { X = 1, Y = 2 } });
            var thing2 = new Spatial2DThing<string>(new[] { new Point2D() { X = 1, Y = 1 } });

            thing1.Data = "A";
            thing2.Data = "A";

            Assert.That(thing1.GetHashCode() != thing2.GetHashCode());
        }

        [Test]
        public void Overlaps_ReturnsTrueWhenPositionsOverlap()
        {
            var thing1 = new Spatial2DThing<string>(new[] { new Point2D() { X = 1, Y = 1 } });
            var thing2 = new Spatial2DThing<string>(new[] { new Point2D() { X = 1, Y = 1 }, new Point2D() { X = 1, Y = 1 } });

            Assert.That(thing1.Overlaps(thing2));
        }

        [Test]
        public void Modified_FiresWhenValueChanged()
        {
            var thing1 = new Spatial2DThing<string>(new[] { new Point2D() { X = 1, Y = 2 } });
            bool evFired = false;

            thing1.Data = "A";

            thing1.Modified += (s, e) =>
            {
                evFired = true;
            };

            thing1.Data = "A";

            Assert.That(evFired, Is.False);

            thing1.Data = "B";

            Assert.That(evFired);
        }
    }
}
