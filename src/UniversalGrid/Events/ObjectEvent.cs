using System;

namespace UniversalGrid.Events
{
    public class ObjectEvent<T> : EventArgs
    {
        public ObjectEvent(T target)
        {
            Target = target;
        }

        public T Target { get; private set; }
    }
}
