using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerScanController : MonoBehaviour
{
    
    private static System.Random random = new System.Random();
    
    [Header("Components")]
    [SerializeField] private StoreItemsController storeItemsController;
    [SerializeField] private ProductListController productListController;
    [SerializeField] private PlayerScore playerScore;
    
    [Header("Configuration")]
    [SerializeField] private int scanListSize = 4;

    private Queue<GameObject> scanList;
    private List<GameObject> scannedObjects = new List<GameObject>();
    
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
            .OrderBy(a => random.Next());

        scanList = new Queue<GameObject>(storeItems);
     
        productListController.SetProducts(GetScanListNames());
    }

    public List<GameObject> GetScanList()
    {
        var scanList = new List<GameObject>();
        for (int i = 0; i < scanListSize; i++)
        {
            if (this.scanList.Count > 0)
            {
                scanList.Add(this.scanList.Dequeue());
            }
        }

        return scanList;
    }

    public bool Scan(GameObject item)
    {
        
        // Check if item is in the 4 first items
        var displayedScanList = GetScanList().GetRange(0,4);
        var isItemInScanList = scanList.Contains(item);
        
        if (!isItemInScanList)
        {
            return false;
        }
        
        var itemIndex = displayedScanList.FindIndex(a => a == item);
        scannedObjects.Add(item);
        
        scanList = new Queue<GameObject>(scanList.Where(a => a != item));
        productListController.CheckAndReplace(itemIndex, GetScanListNames());
        playerScore.Increment(1);
        
        return true;
    }
    
    public List<GameObject> GetScannedObjects()
    {
        return new List<GameObject>(scannedObjects);
    }

    public List<String> GetScanListNames()
    {
        return GetScanList()
            .Select(a => a.name)
            .ToList();
    }

}
