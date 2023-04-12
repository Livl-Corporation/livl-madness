using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfController : MonoBehaviour
{
    public List<GameObject> spawnPoints;
    private List<int> usedSpawnPointsIdx = new List<int>();

    public void setItem(GameObject item)
    {

        if (this.usedSpawnPointsIdx.Count == this.spawnPoints.Count)
        {
            return;
        }

        int rndSpawnIdx;
        do
        {
            rndSpawnIdx = Random.Range(0, spawnPoints.Count);
        } while (usedSpawnPointsIdx.Contains(rndSpawnIdx));

        usedSpawnPointsIdx.Add(rndSpawnIdx);

        GameObject spawnPoint = spawnPoints[rndSpawnIdx];
        Vector3 randomSpawnPosition = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, spawnPoint.transform.position.z);


        Instantiate(item, randomSpawnPosition, Quaternion.identity);

    }

    public bool isEmptySpaceAvailable()
    {
        return this.usedSpawnPointsIdx.Count < this.spawnPoints.Count;
    }
}
