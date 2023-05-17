using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerRoomSpawn : NetworkBehaviour
{
    [FormerlySerializedAs("SpawnPoints")] public List<GameObject> spawnPoints;
    private Queue<GameObject> availableSpawnPoints = new Queue<GameObject>();
    private Queue<GameObject> usedSpawnPoints = new Queue<GameObject>();
    [SerializeField] private GameObject playerPrefab;
    private List<GameObject> spawnedPlayers = new List<GameObject>();

    private void Awake()
    {
        availableSpawnPoints = new Queue<GameObject>(spawnPoints.ToList());
    }
    
    public void SpawnPlayerPrefab(string username)
    {
        if (!IsEmptySpaceAvailable())
        {
            Debug.Log("No more available space in the players spaaaaaawn");
            return;
        }
        
        Debug.Log("Spawn PlayerPrefab");
        
        var item = playerPrefab;

        var selectedSpawnPoint = availableSpawnPoints.Dequeue();
        usedSpawnPoints.Enqueue(selectedSpawnPoint);
        var position = selectedSpawnPoint.transform.position;
        var rotationY = selectedSpawnPoint.transform.rotation.eulerAngles.y + item.transform.rotation.eulerAngles.y;
        
        var spawnedObject = Instantiate(item, position, Quaternion.Euler(0f, rotationY, 0f));
        spawnedObject.transform.name = username;
        NetworkServer.Spawn(spawnedObject);
        spawnedPlayers.Add(spawnedObject);
        
        Debug.Log("PlayerPrefab spawned");
    }

    public void UnspawnPlayerPrefab()
    {
        if (spawnedPlayers.Count == 0)
        {
            Debug.Log("No more available space in the players spaaaaaawn");
            return;
        }
        
        var selectedSpawnPoint = spawnedPlayers[^1];
        spawnedPlayers.RemoveAt(0);
        NetworkServer.UnSpawn(selectedSpawnPoint);
        Destroy(selectedSpawnPoint);
        availableSpawnPoints.Enqueue(usedSpawnPoints.Dequeue());
    }

    private bool IsEmptySpaceAvailable()
    {
        return availableSpawnPoints.Count > 0;
    }
}
