using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Models;
using UnityEngine;

public class ScanListController : MonoBehaviour, IProductListObservable
{
    [Header("Components")]
    [SerializeField] private ProductListController productListController;
    [SerializeField] private StoreItemsController storeItemsController;
    
    [Header("Configuration")]
    [SerializeField] public int scanListSize = 4;
    
    private static System.Random random = new System.Random();

    private Queue<ProductItem> scanList;
    private List<ProductItem> scannedObjects = new List<ProductItem>();
    private List<IProductListObserver> observers = new List<IProductListObserver>();

    public void AddObserver(IProductListObserver observer)
    {
        observers.Add(observer);
        NotifyObservers();
    }

    public void RemoveObserver(IProductListObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        observers.ForEach(observer => observer.UpdateProductList(GetScanList()));
    }
    
    public void Start()
    {
        // Find store items controller
        if (storeItemsController == null)
        {
            storeItemsController = FindObjectOfType<StoreItemsController>();
        }
        
        // Find product list controller
        if (productListController == null)
        {
            productListController = FindObjectOfType<ProductListController>();
        }
        
        var storeItems = storeItemsController.GetItems()
            .OrderBy(a => random.Next())
            .Select(a => new ProductItem(a.name));

        scanList = new Queue<ProductItem>(storeItems);
     
        AddObserver(productListController);
    }
    
    
    public List<ProductItem> GetScanList()
    {
        var returnedList = new List<ProductItem>();
        var clonedScanList = new Queue<ProductItem>(scanList);
        for (var i = 0; i < scanListSize; i++)
        {
            if (clonedScanList.Count > 0)
            {
                returnedList.Add(clonedScanList.Dequeue());
            }
        }

        return returnedList;
    }
    
    public List<ProductItem> GetScannedObjects()
    {
        return new List<ProductItem>(scannedObjects);
    }
    
    public List<string> GetScanListNames()
    {
        return GetScanList()
            .Select(a => a.name)
            .ToList();
    }

    public bool CanBeScanned(string itemName)
    {
        return !GetScanListNames().Contains(itemName);
    }

    public void ScanArticle(string itemName)
    {
        // Add item to scanned objects
        scannedObjects.Add(new ProductItem(itemName));
        
        // Remove scanned items from scanlist
        scanList = new Queue<ProductItem>(scanList.Where(a => a.name != itemName));

        NotifyObservers();
        
    }

}
