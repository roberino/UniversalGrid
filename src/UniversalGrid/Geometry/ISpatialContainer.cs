using System;
using System.Collections.Generic;
using UniversalGrid.Events;

namespace UniversalGrid.Geometry
{
    public interface ISpatialContainer<T>
    {
        event EventHandler<ObjectEvent<ISpatial2DThing<T>>> ItemAdded;
        event EventHandler<ObjectEvent<ISpatial2DThing<T>>> ItemMoved;
        event EventHandler<ObjectEvent<ISpatial2DThing<T>>> ItemRemoved;
        event EventHandler Modified;

        IEnumerable<ISpatial2DThing<T>> AllObjects { get; }
        bool AllowOverlapping { get; set; }

        IEnumerable<ISpatial2DThing<T>> GetObjectsAt(Point2D topLeftPosition);
        IEnumerable<ISpatial2DThing<T>> GetObjectsAt(int x, int y);
        IEnumerable<ISpatial2DThing<T>> GetObjectsOverlapping(Point2D position);
        IEnumerable<ISpatial2DThing<T>> GetObjectsWithin(Rectangle rectangle);

        void SetObject(ISpatial2DThing<T> thing);
        ISpatial2DThing<T> SetObject(T item, int x, int y);
        void SetObjects(params ISpatial2DThing<T>[] things);
        void SetObjects(IEnumerable<ISpatial2DThing<T>> things);

        bool RemoveObject(ISpatial2DThing<T> thing);
    }
}
