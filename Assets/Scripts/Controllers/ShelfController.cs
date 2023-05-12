using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using UnityEngine.Serialization;

public class ShelfController : NetworkBehaviour
{
    [FormerlySerializedAs("SpawnPoints")] public List<GameObject> spawnPoints;
    private Queue<GameObject> availableSpawnPoints = new Queue<GameObject>();

    private void Awake()
    {
        var rnd = new System.Random();
        availableSpawnPoints = new Queue<GameObject>(spawnPoints
            .OrderBy(a => rnd.Next()).ToList());
    }
    
    public GameObject SpawnProduct(GameObject item)
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

    public bool IsEmptySpaceAvailable()
    {
        return availableSpawnPoints.Count > 0;
    }
}
