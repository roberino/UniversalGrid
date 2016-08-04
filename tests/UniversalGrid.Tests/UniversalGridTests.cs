using NUnit.Framework;
using System.Linq;
using UniversalGrid.Geometry;

namespace UniversalGrid.Tests
{
    [TestFixture]
    public class UniversalGridTests
    {
        [Test]
        public void CreateInstance_IsCorrectlyInitialised()
        {
            var grid = new UniversalGrid<string>(10, 20);

            Assert.That(grid.Rows.Count(), Is.EqualTo(20));
            Assert.That(grid.Rows.All(r => r.Count() == 10));
            Assert.That(grid.Height, Is.EqualTo(20));
            Assert.That(grid.Width, Is.EqualTo(10));
            Assert.That(grid.Positions.Count(), Is.EqualTo(10 * 20));
            Assert.That(grid.AllObjects.Any(), Is.False);
        }

        [Test]
        public void SetObject_AtZero_FiredItemAddedEvent()
        {
            var grid = new UniversalGrid<string>(10, 20);

            var thing1 = new Spatial2DThing<string>(new Point2D());

            bool evFired = false;

            grid.ItemAdded += (s, e) =>
            {
                Assert.That(e.Target, Is.SameAs(thing1));
                evFired = true;
            };

            grid.SetObject(thing1);

            Assert.That(evFired);
        }

        [Test]
        public void J()
        {
            var grid = new UniversalGrid<string>(10, 20);

            var thing1 = new Spatial2DThing<string>(new Point2D());

            bool evFired = false;

            grid.ItemAdded += (s, e) =>
            {
                Assert.That(e.Target, Is.SameAs(thing1));
                evFired = true;
            };

            grid.SetObject(thing1);

            Assert.That(evFired);
        }
    }
}