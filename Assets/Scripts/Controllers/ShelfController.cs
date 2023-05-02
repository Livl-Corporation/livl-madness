using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;

public class ShelfController : NetworkBehaviour
{
    public List<GameObject> SpawnPoints;
    private Queue<GameObject> availableSpawnPoints = new Queue<GameObject>();

    private void Awake()
    {
        var rnd = new System.Random();
        availableSpawnPoints = new Queue<GameObject>(SpawnPoints
            .OrderBy(a => rnd.Next()).ToList());
    }
    

    public void SpawnProduct(GameObject item)
    {

        if (!IsEmptySpaceAvailable())
        {
            Debug.LogError("No more available space in this shelve");
            return;
        }

        var selectedSpawnPoint = availableSpawnPoints.Dequeue();
        var position = selectedSpawnPoint.transform.position;
        
        var spawnedObject = Instantiate(item, position, Quaternion.identity);
        spawnedObject.name = item.name;
        NetworkServer.Spawn(spawnedObject);
        
    }

    public bool IsEmptySpaceAvailable()
    {
        return availableSpawnPoints.Count > 0;
    }
}
