using System;
using System.Collections.Generic;
using System.Linq;
using UniversalGrid.Events;
using UniversalGrid.Geometry;

namespace UniversalGrid
{
    public class UniversalGrid<T> : Rectangle
    {
        private readonly object _lockObj = new object();
        private readonly IDictionary<Point2D, IList<Spatial2DThing<T>>> _items;

        public UniversalGrid(int width, int height) : base(new Point2D(), width, height)
        {
            _items = new Dictionary<Point2D, IList<Spatial2DThing<T>>>();
        }

        /// <summary>
        /// When set to true, objects can overlap each other
        /// </summary>
        public bool AllowOverlapping { get; set; }

        /// <summary>
        /// Fires when a new item is added
        /// </summary>
        public event EventHandler<ObjectEvent<Spatial2DThing<T>>> ItemAdded;

        /// <summary>
        /// Returns rows of points within the grid
        /// </summary>
        public IEnumerable<IEnumerable<Point2D>> Rows
        {
            get
            {
                return Enumerable.Range(0, Height).Select(y => Enumerable.Range(0, Width).Select(x => new Point2D() { X = x, Y = y }));
            }
        }

        /// <summary>
        /// Returns all objects on the grid
        /// </summary>
        public IEnumerable<Spatial2DThing<T>> AllObjects
        {
            get
            {
                return _items.SelectMany(x => x.Value);
            }
        }

        /// <summary>
        /// Returns all objects which overlap a specified point
        /// </summary>
        public IEnumerable<Spatial2DThing<T>> GetObjectsOverlapping(Point2D position)
        {
            return _items.Values.SelectMany(v => v.Where(x => x.Overlaps(position)));
        }

        /// <summary>
        /// Returns all objects within the boundy of a rectange
        /// </summary>
        public IEnumerable<Spatial2DThing<T>> GetObjectsWithin(Rectangle rectangle)
        {
            return _items.Values.SelectMany(v => v.Where(x => x.IsWithin(rectangle)));
        }

        public IEnumerable<Spatial2DThing<T>> GetObjectsAt(Point2D topLeftPosition)
        {
            lock (_lockObj)
            {
                IList<Spatial2DThing<T>> items;

                if (!_items.TryGetValue(topLeftPosition, out items))
                {
                    return Enumerable.Empty<Spatial2DThing<T>>();
                }

                return items;
            }
        }

        public void SetObjects(params Spatial2DThing<T>[] things)
        {
            SetObjects((IEnumerable<Spatial2DThing<T>>)things);
        }


        /// <summary>
        /// Places multiple items onto the grid. If any of the operations are invalid, the whole operation fails with an exception.
        /// </summary>
        public void SetObjects(IEnumerable<Spatial2DThing<T>> things)
        {
            lock (_lockObj)
            {
                foreach (var t in things)
                {
                    SetObject(t, true); // ensure all can be added before actual add
                }

                foreach (var t in things)
                {
                    SetObject(t, false);
                }
            }
        }


        /// <summary>
        /// Places an item onto the grid at the given coordinates
        /// </summary>
        public Spatial2DThing<T> SetObject(T item, int x, int y)
        {
            var thing = new Spatial2DThing<T>(new Point2D() { X = x, Y = y }) { Data = item };

            SetObject(thing);

            return thing;
        }

        /// <summary>
        /// Places a spatial thing onto the grid
        /// </summary>
        public void SetObject(Spatial2DThing<T> thing)
        {
            SetObject(thing, false);
        }

        private void SetObject(Spatial2DThing<T> thing, bool simulate)
        {
            lock (_lockObj)
            {
                IList<Spatial2DThing<T>> items;

                if (!_items.TryGetValue(thing.TopLeft, out items))
                {
                    _items[thing.TopLeft] = items = new List<Spatial2DThing<T>>();
                }

                if (!Overlaps(thing))
                {
                    throw new ObjectOutOfBoundsException(thing);
                }

                if (items.Contains(thing))
                {
                    throw new InvalidOperationException("Item already added to grid");
                }

                if (!AllowOverlapping && (items.Any() || _items.Values.Any(m => m.Any(x => x.Overlaps(thing)))))
                {
                    throw new InvalidOperationException("Item overlaps existing item");
                }

                if (!simulate)
                {
                    items.Add(thing);

                    thing.BeforeMoved += (s, e) =>
                    {
                        SetObject((Spatial2DThing<T>)s, true);
                    };

                    thing.Moved += (s, e) =>
                    {
                        items.Remove((Spatial2DThing<T>)s);

                        SetObject((Spatial2DThing<T>)s);
                    };
                }
            }

            if (!simulate)
            {
                var ev = ItemAdded;

                if (ev != null)
                {
                    ev.Invoke(this, new ObjectEvent<Spatial2DThing<T>>(thing));
                }
            }
        }
    }
}