using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Models;
using UnityEngine;

public class PlayerScanController : MonoBehaviour, IProductListObservable
{
    
    private static System.Random random = new System.Random();
    
    [Header("Components")]
    [SerializeField] private StoreItemsController storeItemsController;
    [SerializeField] private ProductListController productListController;
    [SerializeField] private PlayerScore playerScore;
    
    [Header("Configuration")]
    [SerializeField] private int scanListSize = 4;

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
        
        // Find player score
        if (playerScore == null)
        {
            playerScore = FindObjectOfType<PlayerScore>();
        }
        
        var storeItems = storeItemsController.GetItems()
            .OrderBy(a => random.Next())
            .Select(a => new ProductItem(a.name));

        scanList = new Queue<ProductItem>(storeItems);
     
        AddObserver(productListController);
    }

    private List<ProductItem> GetScanList()
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

    public bool Scan(GameObject item)
    {
        
        var scanListIdentities = GetScanListNames();
        
        if (!scanListIdentities.Contains(item.name))
        {
            return false;
        }
        
        // Add item to scanned objects
        scannedObjects.Add(new ProductItem(item.name));
        
        // Remove scanned items from scanlist
        scanList = new Queue<ProductItem>(scanList.Where(a => a.name != item.name));
        
        // Update UI
        NotifyObservers();
        playerScore.Increment(1);
        
        return true;
    }
    
    public List<ProductItem> GetScannedObjects()
    {
        return new List<ProductItem>(scannedObjects);
    }

    private List<string> GetScanListNames()
    {
        return GetScanList()
            .Select(a => a.name)
            .ToList();
    }
    
}
