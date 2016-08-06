using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UniversalGrid.Drawing;
using UniversalGrid.Events;

namespace UniversalGrid.Geometry
{
    public class Spatial2DThing<T> : ISpatial2DThing<T>
    {
        private T _data;
        private bool _selected;
        private Point2D? _rotationalCentre;

        public Spatial2DThing(IEnumerable<Point2D> positions)
        {
            Contract.Assert(positions.Any());

            Positions = positions.OrderBy(p => p.Y).ThenBy(p => p.X).ToList();
        }

        public Spatial2DThing(Point2D position)
        {
            Positions = new Point2D[] { position };
        }

        public string Label { get; set; }

        public Colour Colour { get; set; }

        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                if (_selected != value)
                {
                    _selected = value;

                    var ev = SelectionChanged;

                    if (ev != null) ev.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler<ObjectEvent<T>> Modified;

        public event EventHandler SelectionChanged;

        public event EventHandler<Point2DEventArgs> BeforeMoved;

        public event EventHandler<Point2DEventArgs> Moved;

        public Point2D RotationalCentre
        {
            get
            {
                return _rotationalCentre.GetValueOrDefault(Centre);
            }
            set
            {
                _rotationalCentre = value;
            }
        }

        public Point2D Centre
        {
            get
            {
                var mx = Positions.Min(p => p.X);
                var my = Positions.Min(p => p.Y);
                var x = mx + (Positions.Max(p => p.X) - mx) / 2;
                var y= my + (Positions.Max(p => p.Y) - my) / 2;

                return new Point2D() { X = x, Y = y };
            }
        }

        public Point2D TopLeft
        {
            get
            {
                return Positions.FirstOrDefault();
            }
        }

        public IEnumerable<Point2D> Positions { get; internal set; }

        public T Data
        {
            get { return _data; }
            set
            {
                var orig = _data;
                var isDefault = orig == null;

                _data = value;

                if (isDefault && _data != null)
                {
                    FireModifiedEvent();
                }

                if (!isDefault && _data != null)
                {
                    if (!orig.Equals(_data))
                    {
                        FireModifiedEvent();
                    }
                }
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Spatial2DThing<T>);
        }

        public bool Equals(ISpatial2DThing<T> other)
        {
            if (other == null) return false;

            if (ReferenceEquals(other, this)) return true;

            if (other.Data == null && Data != null) return false;

            if (!other.Data.Equals(Data)) return false;

            if (other.Positions.Count() != Positions.Count()) return false;

            return other.Positions.Zip(Positions, (p1, p2) => p1.Equals(p2)).All(x => x);
        }

        public override int GetHashCode()
        {
            var comp = StructuralComparisons.StructuralEqualityComparer.GetHashCode(Positions.ToArray());

            if (_data == null) return comp;

            return comp ^ (_data.GetHashCode() * 7);
        }

        public bool Rotate(Point2D? origin = null, int angle = 90)
        {
            var o = origin.GetValueOrDefault(RotationalCentre);

            var newPos = Positions.Select(p => p.Rotate(o, angle)).ToList();

            var ev = BeforeMoved;

            var eva = new Point2DEventArgs(newPos);
            if (ev != null) ev.Invoke(this, eva);

            if (eva.Abort) return false;

            Positions = newPos;

            var ev2 = Moved;

            if (ev2 != null) ev2.Invoke(this, eva);

            return true;
        }

        public bool Modify(IEnumerable<Point2D> newPosition)
        {
            if (!(Positions.Except(newPosition).Any() || newPosition.Except(Positions).Any())) return false;
            var ev = BeforeMoved;

            var eva = new Point2DEventArgs(newPosition);
            if (ev != null) ev.Invoke(this, eva);

            if (eva.Abort) return false;

            Positions = newPosition;

            var ev2 = Moved;

            if (ev2 != null) ev2.Invoke(this, new Point2DEventArgs(newPosition));

            return true;
        }

        /// <summary>
        /// Moves in the specified direction by the specified amount
        /// </summary>
        public bool Move(Direction direction, int amount = 1)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Move(0, -amount);
                case Direction.Down:
                    return Move(0, amount);
                case Direction.Left:
                    return Move(-amount, 0);
                case Direction.Right:
                    return Move(amount, 0);
            }

            return false;
        }

        /// <summary>
        /// Moves the thing in the direction of the specified x and y coordinates
        /// </summary>
        public bool Move(int x, int y)
        {
            return Move(new Point2D() { X = x, Y = y });
        }

        /// <summary>
        /// Moves the thing in the direction of the specified vector
        /// </summary>
        public bool Move(Point2D vector)
        {
            var newPos = Positions.Select(p => p.Translate(vector)).ToList();
            var ev = BeforeMoved;

            var eva = new Point2DEventArgs(newPos);
            if (ev != null) ev.Invoke(this, eva);

            if (eva.Abort) return false;

            Positions = newPos;

            var ev2 = Moved;

            if (ev2 != null) ev2.Invoke(this, new Point2DEventArgs(newPos));

            if (_rotationalCentre.HasValue)
            {
                _rotationalCentre = _rotationalCentre.Value.Translate(vector);
            }

            return true;
        }

        public bool Overlaps(IEnumerable<Point2D> positions)
        {
            var overlapping = Positions.Intersect(positions, Point2DEqualityComparer.Comparer).ToList();

            return overlapping.Any();
        }

        public bool Overlaps(Point2D position)
        {
            return Positions.Any(p => p.Equals(position));
        }

        public bool Overlaps(ISpatial2D spatial)
        {
            return Overlaps(spatial.Positions);
        }

        /// <summary>
        /// Returns true if this object is wholely within the bounds of the other spatial object
        /// </summary>
        public bool IsWithin(ISpatial2D spatial)
        {
            //TODO: Could be optimised for simple shapes

            return !(Positions.Except(spatial.Positions).Any());
        }

        private void FireModifiedEvent()
        {
            var ev = Modified;

            if (ev != null)
            {
                ev.Invoke(this, new ObjectEvent<T>(_data));
            }
        }
    }
}
