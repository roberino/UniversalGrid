using System;
using System.Collections.Generic;
using UniversalGrid.Drawing;
using UniversalGrid.Events;

namespace UniversalGrid.Geometry
{
    /// <summary>
    /// Represents a thing which consumes a 2 dimensional space
    /// </summary>
    public interface ISpatial2DThing<T> : ISpatial2D, IEquatable<ISpatial2DThing<T>>
    {
        /// <summary>
        /// Gets or sets a colour
        /// </summary>
        Colour Colour { get; set; }

        /// <summary>
        /// Gets or sets the data
        /// </summary>
        T Data { get; set; }

        /// <summary>
        /// Annotates the object with an id
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Annotates the object with a label
        /// </summary>
        string Label { get; set; }

        /// <summary>
        /// Gets or sets a flag to mark the object as selected
        /// </summary>
        bool Selected { get; set; }

        /// <summary>
        /// Gets or sets a point around which the object can be rotated
        /// </summary>
        Point2D RotationalCentre { get; set; }

        /// <summary>
        /// Fires before an attempt is made to move the object
        /// </summary>
        event EventHandler<Point2DEventArgs> BeforeMoved;

        /// <summary>
        /// Fires after an object is moved
        /// </summary>
        event EventHandler<Point2DEventArgs> Moved;

        /// <summary>
        /// Fires when an object's data is modified
        /// </summary>
        event EventHandler<ObjectEvent<T>> Modified;

        /// <summary>
        /// Fires when the objects selected state is changed
        /// </summary>
        event EventHandler SelectionChanged;

        /// <summary>
        /// Moves the object in the direction and magnitude of x,y
        /// </summary>
        bool Move(int x, int y);

        /// <summary>
        /// Moves the object in the direction and amount specified
        /// </summary>
        bool Move(Direction direction, int amount = 1);

        /// <summary>
        /// Rotates the object
        /// </summary>
        /// <param name="origin">An optional origin (defaults to the RotationalCentre)</param>
        /// <param name="angle">An optional angle</param>
        bool Rotate(Point2D? origin = null, int angle = 90);

        /// <summary>
        /// Modifies the object's positions, by replacing with new positions
        /// </summary>
        bool Modify(IEnumerable<Point2D> newPosition);
    }
}