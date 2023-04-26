namespace Interfaces
{
    public interface IProductListObservable
    {
        void AddObserver(IProductListObserver observer);
        void RemoveObserver(IProductListObserver observer);
        void NotifyObservers();
    }
}