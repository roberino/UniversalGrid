using NUnit.Framework;
using System;
using System.Linq;
using UniversalGrid.Geometry;
using UniversalGrid.Rules;

namespace UniversalGrid.Tests
{
    [TestFixture]
    public class UniversalGridTests
    {
        [Test]
        public void CreateInstance_InitialisesAsExpected()
        {
            var grid = new UniversalGrid<string>(10, 20);

            Assert.That(grid.Rows.Count(), Is.EqualTo(20));
            Assert.That(grid.Rows.First().First().X, Is.EqualTo(0));
            Assert.That(grid.Rows.Last().Last().X, Is.EqualTo(9));
            Assert.That(grid.Rows.First().First().Y, Is.EqualTo(0));
            Assert.That(grid.Rows.Last().Last().Y, Is.EqualTo(19));
            Assert.That(grid.Rows.All(r => r.Count() == 10));
            Assert.That(grid.Height, Is.EqualTo(20));
            Assert.That(grid.Width, Is.EqualTo(10));
            Assert.That(grid.Positions.Count(), Is.EqualTo(10 * 20));
            Assert.That(grid.AllObjects.Any(), Is.False);
        }

        [Test]
        public void SetObject_AtZero_FiredItemAddedAndModifiedEvent()
        {
            var grid = new UniversalGrid<string>(10, 20);

            var thing1 = new Spatial2DThing<string>(new Point2D());

            bool evFired = false;

            grid.ItemAdded += (s, e) =>
            {
                Assert.That(e.Target, Is.SameAs(thing1));
                evFired = true;
            };

            var modified = false;

            grid.Modified += (s, e) =>
            {
                modified = true;
            };

            grid.SetObject(thing1);

            Assert.That(modified);
            Assert.That(evFired);
        }

        [Test]
        public void SetObjectThenMove_MovesWithinGrid()
        {
            var grid = new UniversalGrid<string>(10, 20);

            var thing1 = "A".AsSpatialObject(1, 1);

            var movedFired = false;

            grid.ItemMoved += (s, e) =>
            {
                movedFired = true;
            };

            grid.SetObject(thing1);

            Assert.That(grid.GetObjectsAt(1, 1).Single(), Is.SameAs(thing1));

            thing1.Move(0, 1);

            Assert.That(grid.GetObjectsAt(1,1).Any(), Is.False);
            Assert.That(grid.GetObjectsAt(1, 2).Single(), Is.SameAs(thing1));
            Assert.That(movedFired);
        }

        [Test]
        public void GetObjectsWithin_ReturnsCorrectObject()
        {
            var grid = new UniversalGrid<string>(10, 20);

            var thing1 = "A".AsSpatialObject(1, 1);

            grid.SetObject(thing1);

            Assert.That(grid.GetObjectsWithin(new Rectangle(0, 0, 5, 5)).Single(), Is.EqualTo(thing1));
        }

        [Test]
        public void GetObjectsWithin_ReturnsCorrectObjects()
        {
            var grid = new UniversalGrid<string>(10, 20);

            var thing1 = "A".AsSpatialObject(1, 1);
            var thing2 = "B".AsSpatialObject(1, 2);

            grid.SetObjects(thing1, thing2);

            var objs = grid.GetObjectsWithin(new Rectangle(0, 0, 5, 5)).ToList();

            Assert.That(objs.First(), Is.EqualTo(thing1));
            Assert.That(objs.Skip(1).First(), Is.EqualTo(thing2));
            Assert.That(objs.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetObjectsOverlapping_ReturnsCorrectObject()
        {
            var grid = new UniversalGrid<string>(10, 20);

            var thing1 = "A".AsSpatialObject(1, 1);
            var thing2 = "B".AsSpatialObject(1, 2);

            grid.SetObjects(thing1, thing2);

            var objs = grid.GetObjectsOverlapping(new Point2D() { X = 1, Y = 2 }).ToList();

            Assert.That(objs.Single(), Is.EqualTo(thing2));
        }

        [Test]
        public void Move_OutOfBounds_ThrowsError()
        {
            var grid = new UniversalGrid<string>(3, 3);

            var thing1 = "A".AsSpatialObject(1, 1);

            grid.SetObject(thing1);

            thing1.Move(Direction.Down); // 1, 2

            Assert.Throws<ObjectOutOfBoundsException>(() => thing1.Move(Direction.Down)); // 1, 3
        }

        [Test]
        public void Move_NonTypes_RuleViolation_RaisesEvent()
        {
            var grid = new UniversalGrid<string>(3, 3);

            var thing1 = "A".AsSpatialObject(1, 1);

            var rule = grid.AddConstraint((x, m) => m.Any(p => p.Y > 1)); // Add a rule which prevents Y from exceeding 2

            bool wasExecuted = false;

            grid.RuleViolated += (s, e) =>
            {
                wasExecuted = true;

                Assert.That(e.Rule, Is.SameAs(rule));
            };

            grid.SetObject(thing1);

            thing1.Move(Direction.Down); // try to move => 1, 2

            Assert.That(rule.Id == 1);
            Assert.That(wasExecuted);
        }

        [Test]
        public void Move_RuleViolation_RaisesEvent()
        {
            var grid = new UniversalGrid<string>(3, 3);

            var thing1 = "A".AsSpatialObject(1, 1);

            ISpatialRule rule = null;

            grid.RuleViolated += (s, e) =>
            {
                rule = e.Rule;
            };

            grid.AddConstraint((x, m) => m.Any(p => p.Y > 1), 1, 23); // Add a rule which prevents Y from exceeding 2

            grid.SetObject(thing1);

            thing1.Move(Direction.Right); // 2, 1

            Assert.That(rule == null);

            thing1.Move(Direction.Down); // try to move => 2, 2

            Assert.That(thing1.TopLeft.Y, Is.EqualTo(1), "The Y value should remain unchanged");
            Assert.That(thing1.TopLeft.X, Is.EqualTo(2), "The X value should remain unchanged");

            Assert.That(rule.Id == 23);
        }

        [Test]
        public void Move_InvokesAction()
        {
            var grid = new UniversalGrid<string>(3, 3);

            var thing1 = "A".AsSpatialObject(1, 1);

            bool actionInvoked = false;

            grid.AddAction((x, m) => m.Any(p => p.Y > 1), (g, x) => {
                actionInvoked = true;
            }); // Add a rule which prevents Y from exceeding 2

            grid.SetObject(thing1);

            thing1.Move(Direction.Right); // 2, 1

            Assert.That(actionInvoked, Is.False);

            thing1.Move(Direction.Down); // 2, 2 

            Assert.That(actionInvoked, Is.True);
        }

        [Test]
        public void Move_CorrectlySetsTopLeftPosAndMovesContainingObjects()
        {
            var grid = new UniversalGrid<string>(10, 20);

            var thing1 = "A".AsSpatialObject(1, 1);
            var thing2 = "B".AsSpatialObject(1, 2);
            var moves = 0;
            var modified = false;

            grid.ItemMoved += (s, e) =>
            {
                moves++;
            };

            grid.Modified += (s, e) =>
            {
                modified = true;
            };

            grid.SetObjects(thing1, thing2);

            grid.Move(new Point2D() { X = 2, Y = 1 });

            Assert.That(modified);
            Assert.That(grid.TopLeft, Is.EqualTo(new Point2D() { X = 2, Y = 1 }));
            Assert.That(grid.AllObjects.Count(), Is.EqualTo(2));
            Assert.That(thing1.TopLeft, Is.EqualTo(new Point2D() { X = 3, Y = 2 }));
            Assert.That(thing2.TopLeft, Is.EqualTo(new Point2D() { X = 3, Y = 3 }));
            Assert.That(moves, Is.EqualTo(2));
        }

        [Test]
        public void Render_ToConsole()
        {
            var grid = new UniversalGrid<int>(10, 10);

            var item = grid.SetObject(7, 5, 5);

            item.Label = "X";

            var cellCount = RenderToConsole(grid);

            Assert.That(cellCount, Is.EqualTo(100));
        }

        [Test]
        public void Render_Rotation_ToConsole()
        {
            var grid = new UniversalGrid<int>(10, 10);

            var item = (1).AsSpatialObject(2, 5, new Point2D() { X = 3, Y = 5 });

            item.Label = "X";

            grid.SetObject(item);

            RenderToConsole(grid);

            item.Rotate();
            
            var cellCount = RenderToConsole(grid);

            Assert.That(cellCount, Is.EqualTo(100));
        }

        private int RenderToConsole(UniversalGrid<int> grid)
        {
            Console.WriteLine();

            int c = 0;
            var r = -1;

            grid.Render((p, m) =>
            {
                c++;

                if (p.Y > r)
                {
                    Console.WriteLine();
                    Console.Write(p.Y + "\t");
                }

                Console.Write("|\t");

                if (m.Any())
                {
                    foreach (var i in m)
                    {
                        Console.Write(i.Label);
                    }
                }

                Console.Write("\t");

                r = p.Y;
            });

            Console.WriteLine();

            return c;
        }
    }
}