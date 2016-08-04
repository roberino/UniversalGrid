using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UniversalGrid.Drawing;
using UniversalGrid.Events;

namespace UniversalGrid.Geometry
{
    public class Spatial2DThing<T> : ISpatial2D, IEquatable<Spatial2DThing<T>>
    {
        private T _data;

        private bool _selected;

        public Spatial2DThing(IEnumerable<Point2D> positions)
        {
            Contract.Assert(positions.Any());

            Positions = positions.OrderByDescending(p => p.Y).ThenByDescending(p => p.X).ToList();
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

        public bool Equals(Spatial2DThing<T> other)
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

        public void Move(Point2D vector)
        {
            var ev = BeforeMoved;

            if (ev != null) ev.Invoke(this, new Point2DEventArgs(vector));

            Positions = Positions.Select(p => p.Translate(vector)).ToList();

            var ev2 = Moved;

            if (ev2 != null) ev2.Invoke(this, new Point2DEventArgs(vector));
        }

        public bool Overlaps(Point2D position)
        {
            return Positions.Any(p => p.Equals(position));
        }

        public bool Overlaps(ISpatial2D spatial)
        {
            return (Positions.Intersect(spatial.Positions).Any());
        }

        public bool IsWithin(ISpatial2D spatial)
        {
            return (Positions.Except(spatial.Positions).Any());
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
