using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using UniversalGrid.Events;
using UniversalGrid.Geometry;
using UniversalGrid.Rules;

namespace UniversalGrid
{
    /// <summary>
    /// Represents a 2 dimensional grid which can hold "spatial" objects
    /// </summary>
    /// <typeparam name="T">The type of spatial objects which the grid can contain</typeparam>
    public class UniversalGrid<T> : Rectangle
    {
        private readonly ReaderWriterLockSlim _rwLock;
        private readonly IDictionary<Point2D, IList<ISpatial2DThing<T>>> _items;
        private readonly IDictionary<int, ISpatialRule> _movementRules;
        private readonly IDictionary<int, RuleAction<UniversalGrid<T>>> _actions;
        private Rectangle _viewPort;

        /// <summary>
        /// Creates a new grid, at point (0,0)
        /// </summary>
        /// <param name="width">The width of the grid</param>
        /// <param name="height">The height of the grid</param>
        public UniversalGrid(int width, int height) : base(new Point2D(), width, height)
        {
            _rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            _items = new Dictionary<Point2D, IList<ISpatial2DThing<T>>>();
            _movementRules = new Dictionary<int, ISpatialRule>();
            _actions = new Dictionary<int, RuleAction<UniversalGrid<T>>>();
            _viewPort = new Rectangle(TopLeft, width, height);
        }

        /// <summary>
        /// Gets or sets the "viewport" or visible part of the grid
        /// </summary>
        public Rectangle ViewPort
        {
            get
            {
                return _viewPort;
            }
            set
            {
                Contract.Assert(value != null);

                if (!_viewPort.Equals(value))
                {
                    _viewPort = value;

                    FireModified();
                }
            }
        }

        /// <summary>
        /// When set to true, objects can overlap each other
        /// </summary>
        public bool AllowOverlapping { get; set; }

        public event EventHandler Modified;

        /// <summary>
        /// Fires when a new item is added
        /// </summary>
        public event EventHandler<ObjectEvent<ISpatial2DThing<T>>> ItemAdded;

        /// <summary>
        /// Fires when an item is removed
        /// </summary>
        public event EventHandler<ObjectEvent<ISpatial2DThing<T>>> ItemRemoved;

        /// <summary>
        /// Fires when an item is moved
        /// </summary>
        public event EventHandler<ObjectEvent<ISpatial2DThing<T>>> ItemMoved;

        /// <summary>
        /// Fires when a rule is violated
        /// </summary>
        public event EventHandler<RuleViolationEvent> RuleViolated;

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
        public IEnumerable<ISpatial2DThing<T>> AllObjects
        {
            get
            {
                return Read(() => _items.SelectMany(x => x.Value).ToList());
            }
        }

        /// <summary>
        /// Calls a render function for each cell within the grid,
        /// passing as an argument any overlapping items
        /// </summary>
        public void Render(Action<Point2D, IEnumerable<ISpatial2DThing<T>>> renderer)
        {
            Read(() =>
            {
                foreach (var row in Rows)
                {
                    foreach(var p in row.Where(p => ViewPort.Overlaps(p)))
                    {
                        renderer.Invoke(p, GetObjectsOverlapping(p));
                    }
                }

                return 1;
            });
        }

        /// <summary>
        /// Adds a rule which will be execute before an object is moved
        /// </summary>
        /// <param name="condition">A predicate function which takes a 
        /// thing and a proposed move as arguments and returns a boolean value.
        /// The rule will fire (will be violated) if the condition returns true</param>
        /// <param name="id">An optional id assigned to the rul</param>
        public ISpatialRule AddMovementRule(Func<ISpatial2D, IEnumerable<Point2D>, bool> condition)
        {
            ISpatialRule rule = null;

            Write(() =>
            {
                var idv = _movementRules.Any() ? _movementRules.Max(r => r.Value.Id) + 1 : 1;

                _movementRules[idv] = rule = new TypedSpatialRule<byte>(idv, 0, condition);
            });

            return rule;
        }

        /// <summary>
        /// Adds a (type specific) rule which will be execute before an object is moved
        /// </summary>
        /// <param name="condition">A predicate function which takes a 
        /// thing and a proposed move as arguments and returns a boolean value.
        /// The rule will fire (will be violated) if the condition returns true</param>
        /// <param name="id">An optional id assigned to the rul</param>
        /// <param name="type">The type of rule</param>
        public ISpatialRule AddMovementRule<R>(Func<ISpatial2D, IEnumerable<Point2D>, bool> condition, R type = default(R), int? id = null)
        {
            ISpatialRule rule = null;

            Write(() =>
            {
                var idv = id.HasValue ? id.Value : (_movementRules.Any() ? _movementRules.Max(r => r.Value.Id) + 1 : 1);
                _movementRules[idv] = rule = new TypedSpatialRule<R>(idv, type, condition);
            });

            return rule;
        }

        /// <summary>
        /// Adds an action which will be execute before an object is moved when the condition is met
        /// </summary>
        /// <param name="condition">A predicate function which takes a 
        /// thing and a proposed move as arguments and returns a boolean value.
        /// The rule will fire (will be violated) if the condition returns true</param>
        /// <param name="action">An action which will be invoked when the condition is met</param>
        public void AddAction(Func<ISpatial2D, IEnumerable<Point2D>, bool> condition, Action<UniversalGrid<T>, ISpatial2D> action, int? id = null)
        {
            Write(() =>
            {
                var idv = id.HasValue ? id.Value : (_actions.Any() ? _actions.Max(r => r.Value.Id) + 1 : 1);
                _actions[idv] = new RuleAction<UniversalGrid<T>>(idv, action, condition);
            });
        }

        /// <summary>
        /// Moves the grid's cartesian coordinates and inner items
        /// </summary>
        public override bool Move(Point2D vector)
        {
            bool moved = false;

            Write(() =>
            {
                if (base.Move(vector))
                {
                    foreach (var items in _items.Values.ToList())
                    {
                        foreach (var item in items.ToList())
                        {
                            item.Move(vector);
                        }
                    }

                    foreach (var empty in _items.Where(x => x.Value.Count == 0).ToList())
                    {
                        _items.Remove(empty.Key);
                    }

                    moved = true;
                }
            });

            return moved;
        }

        /// <summary>
        /// Returns all objects which overlap a specified point
        /// </summary>
        public IEnumerable<ISpatial2DThing<T>> GetObjectsOverlapping(Point2D position)
        {
            return Read(() => _items.Values.SelectMany(v => v.Where(x => x.Overlaps(position))).ToList());
        }

        /// <summary>
        /// Returns all objects within the boundy of a rectange
        /// </summary>
        public IEnumerable<ISpatial2DThing<T>> GetObjectsWithin(Rectangle rectangle)
        {
            return Read(() => _items.Values.SelectMany(v => v.Where(x => x.IsWithin(rectangle))).ToList());
        }

        /// <summary>
        /// Returns objects having a top left position specified by the x, y coordinates
        /// </summary>
        public IEnumerable<ISpatial2DThing<T>> GetObjectsAt(int x, int y)
        {
            return GetObjectsAt(new Point2D() { X = x, Y = y });
        }

        /// <summary>
        /// Returns objects located at the specified top left position
        /// </summary>
        public IEnumerable<ISpatial2DThing<T>> GetObjectsAt(Point2D topLeftPosition)
        {
            return Read(() =>
           {
               IList<ISpatial2DThing<T>> items;

               if (!_items.TryGetValue(topLeftPosition, out items))
               {
                   return Enumerable.Empty<ISpatial2DThing<T>>();
               }

               return items.ToList();
           });
        }

        /// <summary>
        /// Places multiple items onto the grid. If any of the operations are invalid, the whole operation fails with an exception.
        /// </summary>
        public void SetObjects(params ISpatial2DThing<T>[] things)
        {
            SetObjects((IEnumerable<ISpatial2DThing<T>>)things);
        }


        /// <summary>
        /// Places multiple items onto the grid. If any of the operations are invalid, the whole operation fails with an exception.
        /// </summary>
        public void SetObjects(IEnumerable<ISpatial2DThing<T>> things)
        {
            _rwLock.EnterWriteLock();

            try
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
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Places an item onto the grid at the given coordinates
        /// </summary>
        public ISpatial2DThing<T> SetObject(T item, int x, int y)
        {
            var thing = new Spatial2DThing<T>(new Point2D() { X = x, Y = y }) { Data = item };

            SetObject(thing);

            return thing;
        }

        /// <summary>
        /// Places a spatial thing onto the grid
        /// </summary>
        public void SetObject(ISpatial2DThing<T> thing)
        {
            SetObject(thing, false);
        }

        /// <summary>
        /// Removes an object from the grid (returns false if the item isn't within the grid)
        /// </summary>
        public bool RemoveObject(ISpatial2DThing<T> thing)
        {
            bool found = false;

            _rwLock.EnterWriteLock();

            try
            {
                var find = _items.Values.Select(v => new { list = v, items = v.Where(x => x.Equals(thing)).ToList() }).ToList();

                foreach (var affected in find)
                {
                    foreach (var item in affected.items)
                    {
                        found |= affected.list.Remove(item);
                    }
                }
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }

            thing.Moved -= ThingMovedEventHandler;
            thing.BeforeMoved -= ThingBeforeMovedEventHandler;

            if (found)
            {
                var ev = ItemRemoved;

                if (ev != null)
                {
                    ev.Invoke(this, new ObjectEvent<ISpatial2DThing<T>>(thing));
                }

                FireModified();
            }

            return found;
        }

        private void SetObject(ISpatial2DThing<T> thing, bool simulate, bool fireAdded = true)
        {
            Write(() =>
            {
                IList<ISpatial2DThing<T>> items;

                if (!_items.TryGetValue(thing.TopLeft, out items))
                {
                    _items[thing.TopLeft] = items = new List<ISpatial2DThing<T>>();
                }

                if (!thing.IsWithin(this))
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

                    thing.BeforeMoved += ThingBeforeMovedEventHandler;

                    thing.Moved += ThingMovedEventHandler;
                }
            });

            if (!simulate && fireAdded)
            {
                var ev = ItemAdded;

                if (ev != null)
                {
                    ev.Invoke(this, new ObjectEvent<ISpatial2DThing<T>>(thing));
                }

                FireModified();
            }
        }

        private void ThingBeforeMovedEventHandler(object s, Point2DEventArgs e)
        {
            var thing = (ISpatial2DThing<T>)s;

            if (e.Points.Any(p => !Overlaps(p)))
            {
                throw new ObjectOutOfBoundsException(thing);
            }

            if (!AllowOverlapping && _items.Values.Any(m => m.Any(x => !x.Equals(thing) && x.Overlaps(e.Points))))
            {
                throw new InvalidOperationException("Item overlaps existing item");
            }

            var violations = _movementRules.Values.Where(r => r.Condition(thing, e.Points)).ToList();

            var ev = RuleViolated;

            foreach (var rule in violations)
            {
                e.Abort = true;

                if (ev == null) return; // No event listener

                ev(this, new RuleViolationEvent(rule, thing));
            }

            var actions = _actions.Values.Where(r => r.Condition(thing, e.Points)).ToList();

            foreach (var action in actions)
            {
                action.RuleType.Invoke(this, thing);
            }
        }

        private void ThingMovedEventHandler(object s, Point2DEventArgs e)
        {
            RemoveObject((ISpatial2DThing<T>)s);

            SetObject((ISpatial2DThing<T>)s, false, false);

            FireItemMoved((ISpatial2DThing<T>)s);
        }

        private void FireItemMoved(ISpatial2DThing<T> item)
        {
            var ev = ItemMoved;

            if (ev != null)
            {
                ev.Invoke(this, new ObjectEvent<ISpatial2DThing<T>>(item));
            }

            FireModified();
        }

        private void FireModified()
        {
            var ev = Modified;

            if (ev != null)
            {
                ev.Invoke(this, EventArgs.Empty);
            }
        }

        private TX Read<TX>(Func<TX> operation)
        {
            _rwLock.EnterReadLock();

            try
            {
                return operation();
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        private void Write(Action operation)
        {
            _rwLock.EnterWriteLock();

            try
            {
                operation();
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }
    }
}