using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Interfaces;
using Mirror;
using Models;
using UnityEngine;

public class ScanListController : NetworkBehaviour, IProductListObservable
{
    [Header("Components")]
    [SerializeField] private ProductsController productsController;
    
    [Header("Configuration")]
    [SerializeField] public int scanListSize = 4;
    [SerializeField] private int newProductDelay = 3;
    
    private static System.Random random = new System.Random();

    private readonly List<ProductItem> scanList = new List<ProductItem>();
    private readonly SyncList<ProductItem> syncScanList = new SyncList<ProductItem>();

    private List<IProductListObserver> observers = new List<IProductListObserver>();

    public void Start()
    {
        // Find store items controller
        if (productsController == null)
        {
            productsController = FindObjectOfType<ProductsController>();
        }

        syncScanList.Callback += (op, index, item, newItem) => NotifyObservers();
        
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        
        // Server populate the scan list
        
        var productItems = productsController.GetItems()
            .OrderBy(a => random.Next())
            .Select(a => new ProductItem(a.gameObject.GetComponent<StoreItemPrefabConfigurator>().itemDisplayedName))
            .ToList();
        
        scanList.AddRange(productItems);

        UpdateSyncList();

    }

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

    public List<ProductItem> GetScanList()
    {
        return syncScanList.ToList();
    }
    
    private void UpdateSyncList() {
        var scanListClone = new List<ProductItem>(scanList);
        var scanListToShare = scanListClone.Count >= scanListSize 
            ? scanListClone.GetRange(0, scanListSize) 
            : scanListClone;
        syncScanList.Clear();
        syncScanList.AddRange(scanListToShare);
    }
    
    public List<string> GetScanListNames()
    {
        return GetScanList()
            .Select(a => a.Name)
            .ToList();
    }

    public bool CanBeScanned(string itemName)
    {
        return GetScanList()
            .Where(a => !a.Scanned)
            .Select(a => a.Name)
            .Contains(itemName);
    }

    [Command(requiresAuthority = false)]
    public void CmdScanArticle(string itemName, string scannedBy)
    {
        CheckProduct(itemName, scannedBy);
        StartCoroutine(DelayedDeleteProduct(itemName));
    }

    [Command(requiresAuthority = false)]
    public void CmdUpdateStock()
    {
        var outOfStockProducts = productsController.GetOutOfStockProducts();
        foreach (var productItem in scanList)
        {
            productItem.OutOfStock = outOfStockProducts.Contains(productItem.Name);
        }
        UpdateSyncList();
    }

    private void CheckProduct(string itemName, string scannedBy)
    {
        var productIndex = IndexOfProduct(itemName);
        
        if (productIndex < 0 || productIndex >= scanListSize)
        {
            Debug.LogError("Product index out of range");
            return;
        }

        scanList[productIndex].Scanned = true;
        scanList[productIndex].ScannedBy = scannedBy;
        UpdateSyncList();
    }

    private int IndexOfProduct(string itemName)
    {
        return GetScanListNames().IndexOf(itemName);
    } 

    private void DeleteProduct(string itemName)
    {
        var productIndex = IndexOfProduct(itemName);
        
        if (productIndex < 0 || productIndex >= scanListSize)
        {
            Debug.LogError("Product index out of range");
            return;
        }
        
        var removedItem = scanList[productIndex];
        scanList.RemoveAt(productIndex);
        
        // Put this item randomly in the list after displayed items
        var randomIndex = random.Next(scanListSize, scanList.Count);
        scanList.Insert(randomIndex, new ProductItem(removedItem.Name));
        
        UpdateSyncList();
    }
    
    private IEnumerator DelayedDeleteProduct(string productName)
    {
        yield return new WaitForSeconds(newProductDelay);
        DeleteProduct(productName);
    }
    

}
