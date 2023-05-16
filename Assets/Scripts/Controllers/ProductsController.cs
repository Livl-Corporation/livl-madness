using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;

public class ProductsController : NetworkBehaviour
{
    
    [SerializeField] private int productRespawnMinDelay = 30;  
    [SerializeField] private int productRespawnMaxDelay = 120;
    
    [SerializeField] private List<GameObject> items = new List<GameObject>();
    [SerializeField] private List<ShelfController> shelves = new List<ShelfController> ();
    
    [SerializeField] private ScanListController scanListController;
    
    private readonly SyncDictionary<string, List<GameObject>> spawnedProducts = new SyncDictionary<string, List<GameObject>>();
    private readonly SyncList<string> outOfStockProducts = new SyncList<string>();
    
    private Queue<GameObject> itemSpawnList;
    
    public void Start()
    {
        if (scanListController == null)
        {
            scanListController = FindObjectOfType<ScanListController>();
        }
    }

    private void RegisterSpawnedProduct(GameObject product)
    {
        var storeItem = product.GetComponent<StoreItem>();
        
        if (storeItem == null)
        {
            Debug.LogError($"Product {product.name} does not have a StoreItem component");
            return;
        }
        
        var productName = storeItem.displayedName;

        var productList = new List<GameObject> {product};
        
        if (spawnedProducts.TryGetValue(productName, out var list))
            productList.AddRange(list);
        
        spawnedProducts[productName] = productList;
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
        
        scanListController.CmdUpdateStock();
        
        StartCoroutine(DelayedProductSpawn(productName));
        
    }
    
    [Command(requiresAuthority = false)]
    private void SetInStock(string productName)
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
        
        scanListController.CmdUpdateStock();
    }

    private IEnumerator DelayedProductSpawn(string productName)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(productRespawnMinDelay, productRespawnMaxDelay));
        SetInStock(productName);
    }
    
    public List<string> GetOutOfStockProducts()
    {
        return new List<string>(outOfStockProducts);
    }

    public GameObject GetProductToSpawn()
    {
        var productToSpawn = itemSpawnList.Dequeue();
        itemSpawnList.Enqueue(productToSpawn);
        return productToSpawn;
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
        
        scanListController = FindObjectOfType<ScanListController>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        var rnd = new System.Random();
        
        var availableShelves = new Queue<ShelfController>(shelves
            .OrderBy(a => rnd.Next()).ToList());
        
        itemSpawnList = new Queue<GameObject>(items
            .OrderBy(a => rnd.Next()).ToList());
        
        var spawnedItems = availableShelves
            .SelectMany((shelf) => shelf.FillShelf(this))
            .ToList();
        
        spawnedItems.ForEach(RegisterSpawnedProduct);

    }

    private static GameObject ProductSpawnDelegate(SpawnMessage msg, GameObject item)
    {
        var spawnedObject = Instantiate(item, msg.position, msg.rotation);
        spawnedObject.name = item.name;
        return spawnedObject;
    }

    private static void ProductDestroyDelegate(GameObject spawnedObject)
    {
        Destroy(spawnedObject);
    }

    public List<GameObject> GetItems()
    {
        return new List<GameObject>(items);
    }
    
}
