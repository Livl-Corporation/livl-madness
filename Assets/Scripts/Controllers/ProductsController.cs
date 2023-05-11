using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;

public class ProductsController : NetworkBehaviour
{
    [SerializeField] private List<GameObject> items = new List<GameObject>();
    [SerializeField] private List<ShelfController> shelves = new List<ShelfController> ();
    
    private readonly SyncDictionary<string, List<GameObject>> spawnedProducts = new SyncDictionary<string, List<GameObject>>();
    private readonly SyncList<string> outOfStockProducts = new SyncList<string>();

    private void RegisterSpawnedProduct(GameObject product)
    {
        var storeItem = product.GetComponent<StoreItem>();
        
        if (storeItem == null)
        {
            Debug.LogError($"Product {product.name} does not have a StoreItem component");
            return;
        }
        
        var productName = storeItem.displayedName;
        
        if (!spawnedProducts.ContainsKey(productName))
        {
            spawnedProducts.Add(productName, new List<GameObject>());
        }
        spawnedProducts[productName] = new List<GameObject> {product};
    }
    
    [Command(requiresAuthority = false)]
    public void CmdSetOutOfStock(string productName)
    {
        if (!spawnedProducts.ContainsKey(productName))
        {
            Debug.LogError($"Product {productName} not found in spawned products");
            return;
        }
        outOfStockProducts.Add(productName);
        spawnedProducts[productName].ForEach((obj) =>
        {
            NetworkServer.UnSpawn(obj);
            obj.SetActive(false);
        });
    }
    
    [Command]
    public void SetInStock(string productName)
    {
        if (!spawnedProducts.ContainsKey(productName))
        {
            Debug.LogError($"Product {productName} not found in spawned products");
            return;
        }
        outOfStockProducts.Remove(productName);
        spawnedProducts[productName].ForEach((obj) =>
        {
            NetworkServer.Spawn(obj);
            obj.SetActive(true);
        });
    }

    private void Awake()
    {
        foreach (var item in items)
        {
            NetworkClient.RegisterPrefab(
                item, 
                msg => ProductSpawnDelegate(msg, item),
                ProductDestroyDelegate
            );
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        var rnd = new System.Random();
        var availableShelves = new Queue<ShelfController>(shelves
            .OrderBy(a => rnd.Next()).ToList());

        // Randomly place each item;
        items.ForEach(item =>
        {

            if (availableShelves.Count <= 0)
            {
                Debug.LogError("No more shelves space left for prducts ...");
                return;
            }

            var selectedShelve = availableShelves.Dequeue();
            var spawnedProduct = selectedShelve.SpawnProduct(item);
            
            // Register spawned product
            if (spawnedProduct != null)
            {
                RegisterSpawnedProduct(spawnedProduct);
            }
            
            // If space left on selected shelve, put it again in the list
            if (selectedShelve.IsEmptySpaceAvailable())
                availableShelves.Enqueue(selectedShelve);
            
        });
        
        Debug.Log("Product initialisation finished !");
        
    }
    
    public static GameObject ProductSpawnDelegate(SpawnMessage msg, GameObject item)
    {
        var spawnedObject = Instantiate(item, msg.position, msg.rotation);
        spawnedObject.name = item.name;
        return spawnedObject;
    }
    
    public static void ProductDestroyDelegate(GameObject spawnedObject)
    {
        Destroy(spawnedObject);
    }

    public List<GameObject> GetItems()
    {
        return new List<GameObject>(items);
    }
    
}
