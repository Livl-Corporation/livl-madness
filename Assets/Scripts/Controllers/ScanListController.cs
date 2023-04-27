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

    private readonly SyncList<ProductItem> scanList = new SyncList<ProductItem>();
    
    private List<IProductListObserver> observers = new List<IProductListObserver>();

    public void Start()
    {
        // Find store items controller
        if (productsController == null)
        {
            productsController = FindObjectOfType<ProductsController>();
        }

        scanList.Callback += (op, index, item, newItem) => NotifyObservers();

    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        
        // Server populate the scan list
        
        var productList = productsController.GetItems()
            .OrderBy(a => random.Next())
            .Select(a => new ProductItem(a.name))
            .ToList();
        
        scanList.AddRange(productList);
        
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
        var scanListClone = new List<ProductItem>(scanList);
        return scanListClone.Count >= scanListSize 
            ? scanListClone.GetRange(0, scanListSize) 
            : scanListClone;
    }
    
    public List<string> GetScanListNames()
    {
        return GetScanList()
            .Select(a => a.name)
            .ToList();
    }

    public bool CanBeScanned(string itemName)
    {
        return GetScanList()
            .Where(a => !a.scanned)
            .Select(a => a.name)
            .Contains(itemName);
    }

    [Command(requiresAuthority = false)]
    public void CmdScanArticle(string itemName)
    {

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

        var product = scanList[productIndex];

        scanList[productIndex] = new ProductItem(
            product.name, true, product.isOutOfStock
        );
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
    }
    
    private IEnumerator DelayedDeleteProduct(int productIndex)
    {
        yield return new WaitForSeconds(newProductDelay);
        DeleteProduct(productIndex);
    }
    

}
