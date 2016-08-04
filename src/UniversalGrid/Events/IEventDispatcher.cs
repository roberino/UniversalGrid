namespace UniversalGrid.Events
{
    public interface IEventDispatcher
    {
        void Dispatch<T, E>(T sender, E eventArgs);
    }
}
