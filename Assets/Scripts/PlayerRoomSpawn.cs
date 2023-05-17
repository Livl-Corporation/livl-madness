using System.Collections;
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
        var rnd = new System.Random();
        availableSpawnPoints = new Queue<GameObject>(spawnPoints
            .OrderBy(a => rnd.Next()).ToList());
    }

    [Command(requiresAuthority = false)]
    public void CmdSpawnPlayerPrefab(string username)
    {
        if (!IsEmptySpaceAvailable())
        {
            Debug.Log("No more available space in the players spaaaaaawn");
            return;
        }
        
        var item = playerPrefab;

        var selectedSpawnPoint = availableSpawnPoints.Dequeue();
        usedSpawnPoints.Enqueue(selectedSpawnPoint);
        var position = selectedSpawnPoint.transform.position;
        var rotationY = selectedSpawnPoint.transform.rotation.eulerAngles.y + item.transform.rotation.eulerAngles.y;
        
        var spawnedObject = Instantiate(item, position, Quaternion.Euler(0f, rotationY, 0f));
        spawnedObject.transform.name = username;
        NetworkServer.Spawn(spawnedObject);
        spawnedPlayers.Add(spawnedObject);
    }

    [Command(requiresAuthority = false)]
    public void CmdUnspawnPlayerPrefab()
    {
        if (spawnedPlayers.Count == 0)
        {
            Debug.Log("No more available space in the players spaaaaaawn");
            return;
        }
        
        var selectedSpawnPoint = spawnedPlayers[0];
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
