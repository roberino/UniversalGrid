using System.Linq;

namespace UniversalGrid.Geometry
{
    public static class Extensions
    {
        /// <summary>
        /// Returns a new spatial object from an object instance, specifing x and y coordinates
        /// </summary>
        public static Spatial2DThing<T> ToSpatial2DThing<T>(T item, int x, int y, params Point2D[] positions)
        {
            return new Spatial2DThing<T>(new[] { new Point2D { X = x, Y = y } }.Concat(positions))
            {
                 Data = item
            };
        }
    }
}
