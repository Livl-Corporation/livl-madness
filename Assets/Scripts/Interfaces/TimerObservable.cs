namespace Interfaces
{
    public interface ITimerObservable
    {
        void AddObserver(ITimerObserver observer);
        void RemoveObserver(ITimerObserver observer);
        void NotifyObservers();
    }
}