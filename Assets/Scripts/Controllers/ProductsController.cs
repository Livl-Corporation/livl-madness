using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;

public class ProductsController : NetworkBehaviour
{
    [SerializeField] private List<GameObject> items = new List<GameObject>();
    [SerializeField] private List<ShelfController> shelves = new List<ShelfController> ();
    
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

            NetworkClient.RegisterPrefab(
                item, 
                msg => ProductSpawnDelegate(msg, item),
                ProductDestroyDelegate
            );
            
            var selectedShelve = availableShelves.Dequeue();
            selectedShelve.SpawnProduct(item);
            
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
