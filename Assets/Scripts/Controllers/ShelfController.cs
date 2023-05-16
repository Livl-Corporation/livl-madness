using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using UnityEngine.Serialization;

public class ShelfController : NetworkBehaviour
{
    [FormerlySerializedAs("SpawnPoints")] public List<GameObject> spawnPoints;
    private Queue<GameObject> availableSpawnPoints = new Queue<GameObject>();

    [Header("Configuration")]
    [SerializeField] private int minimumSpawnRate = 20;
    [SerializeField] private int maximumSpawnRate = 60;
    
    private void Awake()
    {
        var rnd = new System.Random();
        availableSpawnPoints = new Queue<GameObject>(spawnPoints
            .OrderBy(a => rnd.Next()).ToList());
    }

    public IEnumerable<GameObject> FillShelf(ProductsController productsController)
    {
        
        var spawnedProducts = new List<GameObject>();
        
        var numberOfSpawn = availableSpawnPoints.Count * Random.Range(minimumSpawnRate, maximumSpawnRate) / 100;
        
        for (var i = 0; i < numberOfSpawn; i++)
        {
            var item = productsController.GetProductToSpawn();
            var spawnedObject = SpawnProduct(item);
            
            if (spawnedObject == null)
            {
                Debug.LogError("No more available space in this shelve");
                break;
            }
            
            spawnedProducts.Add(spawnedObject);
        }
        
        return spawnedProducts;
        
    }
    
    private GameObject SpawnProduct(GameObject item)
    {

        if (!IsEmptySpaceAvailable())
        {
            Debug.LogError("No more available space in this shelve");
            return null;
        }

        var selectedSpawnPoint = availableSpawnPoints.Dequeue();
        var position = selectedSpawnPoint.transform.position;
        var rotationY = selectedSpawnPoint.transform.rotation.eulerAngles.y + item.transform.rotation.eulerAngles.y;
        
        var spawnedObject = Instantiate(item, position, Quaternion.Euler(0f, rotationY, 0f));
        spawnedObject.name = item.name;
        NetworkServer.Spawn(spawnedObject);

        return spawnedObject;
    }

    private bool IsEmptySpaceAvailable()
    {
        return availableSpawnPoints.Count > 0;
    }
}
