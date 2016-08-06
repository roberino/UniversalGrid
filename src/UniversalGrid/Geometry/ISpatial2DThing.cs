﻿using System;
using UniversalGrid.Drawing;
using UniversalGrid.Events;

namespace UniversalGrid.Geometry
{
    public interface ISpatial2DThing<T> : ISpatial2D, IEquatable<ISpatial2DThing<T>>
    {
        Colour Colour { get; set; }
        T Data { get; set; }
        string Label { get; set; }
        bool Selected { get; set; }
        Point2D RotationalCentre { get; set; }

        event EventHandler<Point2DEventArgs> BeforeMoved;
        event EventHandler<ObjectEvent<T>> Modified;
        event EventHandler<Point2DEventArgs> Moved;
        event EventHandler SelectionChanged;

        bool Move(int x, int y);
        bool Move(Direction direction, int amount = 1);
        bool Rotate(Point2D? origin = null, int angle = 90);
    }
}