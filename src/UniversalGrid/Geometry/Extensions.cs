using System.Linq;

namespace UniversalGrid.Geometry
{
    public static class Extensions
    {
        /// <summary>
        /// Returns a new spatial object from an object instance, specifing x and y coordinates
        /// </summary>
        public static ISpatial2DThing<T> AsSpatialObject<T>(this T item, int x, int y, params Point2D[] positions)
        {
            return new Spatial2DThing<T>(new[] { new Point2D { X = x, Y = y } }.Concat(positions))
            {
                 Data = item
            };
        }

        /// <summary>
        /// Returns a new spatial object from an object instance, specifing x and y coordinates and assigning an ID to the object
        /// </summary>
        public static ISpatial2DThing<T> AsSpatialObject<T>(this T item, int x, int y, string id, params Point2D[] positions)
        {
            return new Spatial2DThing<T>(new[] { new Point2D { X = x, Y = y } }.Concat(positions), id)
            {
                Data = item
            };
        }
    }
}
