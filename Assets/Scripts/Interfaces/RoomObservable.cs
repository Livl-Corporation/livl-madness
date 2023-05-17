namespace Interfaces
{
    public interface IRoomObservable
    {
        void AddObserver(IRoomObserver observer);
        void RemoveObserver(IRoomObserver observer);
        void NotifyObservers();
    }
}