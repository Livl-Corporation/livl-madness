using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Interfaces;
using Models;
using UnityEngine;
using UnityEngine.Serialization;

public class ScanListController : MonoBehaviour, IProductListObservable
{
    [Header("Components")]
    [SerializeField] private ProductsController productsController;
    
    [Header("Configuration")]
    [SerializeField] public int scanListSize = 4;
    [SerializeField] private int newProductDelay = 3;
    
    private static System.Random random = new System.Random();

    private List<ProductItem> scanList;
    // TODO : Move scanned object in the player
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
        if (productsController == null)
        {
            productsController = FindObjectOfType<ProductsController>();
        }

        scanList = productsController.GetItems()
            .OrderBy(a => random.Next())
            .Select(a => new ProductItem(a.name))
            .ToList();
     
        // TODO : remove test line
        scanList = scanList.GetRange(0, 5);
        
        // TODO : GameManager should add player as observer
        //AddObserver(productListController);
    }
    
    
    public List<ProductItem> GetScanList()
    {
        return scanList.Count >= scanListSize 
            ? scanList.GetRange(0, scanListSize) 
            : scanList;
    }
    
    public List<ProductItem> GetScannedProducts()
    {
        return new List<ProductItem>(scannedObjects);
    }

    public List<string> GetScannedProductsNames()
    {
        return GetScannedProducts()
            .Select(a => a.name)
            .ToList();
    }  
    
    public List<string> GetScanListNames()
    {
        return GetScanList()
            .Select(a => a.name)
            .ToList();
    }

    public bool CanBeScanned(string itemName)
    {
        return !GetScannedProductsNames().Contains(itemName) 
               && GetScanListNames().Contains(itemName);
    }

    public void ScanArticle(string itemName)
    {
        // Add item to scanned objects
        scannedObjects.Add(new ProductItem(itemName));

        var productIndex = IndexOfProduct(itemName);
        
        CheckProduct(productIndex);

        var coroutine = DelayedDeleteProduct(productIndex);
        StartCoroutine(coroutine);

    }

    private void CheckProduct(int productIndex)
    {
        if (productIndex < 0 || productIndex >= scanListSize)
        {
            Debug.LogError("Product index out of range");
            return;
        }
        
        scanList[productIndex].scanned = true;
        NotifyObservers();
    }

    private int IndexOfProduct(string itemName)
    {
        return GetScanListNames().IndexOf(itemName);
    } 

    private void DeleteProduct(int productIndex)
    {
        if (productIndex < 0 || productIndex >= scanListSize)
        {
            Debug.LogError("Product index out of range");
            return;
        }
        
        scanList.RemoveAt(productIndex);
        NotifyObservers();
    }
    
    private IEnumerator DelayedDeleteProduct(int productIndex)
    {
        yield return new WaitForSeconds(newProductDelay);
        DeleteProduct(productIndex);
    }
    

}
