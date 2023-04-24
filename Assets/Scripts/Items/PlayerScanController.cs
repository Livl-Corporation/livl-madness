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
        var returnedList = new List<GameObject>();
        var clonedScanList = new Queue<GameObject>(scanList);
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
        var itemIndex = scanListIdentities.FindIndex(a => a == item.name);
        scannedObjects.Add(item);
        
        // Remove scanned items from scanlist
        scanList = new Queue<GameObject>(scanList.Where(a => a != item));
        
        // Update UI
        productListController.CheckAndReplace(itemIndex, GetScanListNames());
        playerScore.Increment(1);
        
        return true;
    }
    
    public List<GameObject> GetScannedObjects()
    {
        return new List<GameObject>(scannedObjects);
    }

    private List<string> GetScanListNames()
    {
        return GetScanList()
            .Select(a => a.name)
            .ToList();
    }
    
}
