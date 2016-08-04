using System;

namespace UniversalGrid.Geometry
{
    public class ObjectOutOfBoundsException : ArgumentOutOfRangeException
    {
        public ObjectOutOfBoundsException(ISpatial2D spacialObject)
        {
            Object = spacialObject;
        }

        public ISpatial2D Object { get; private set; }
    }
}
