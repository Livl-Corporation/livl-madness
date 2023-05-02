namespace Interfaces
{
    public interface IPlayerStatsObservable
    {
        void AddObserver(IPlayerStatsObserver observer);
        void RemoveObserver(IPlayerStatsObserver observer);
        void NotifyObservers();
    }
}