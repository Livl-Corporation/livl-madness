using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProductsController : NetworkBehaviour 

{
    [SerializeField] private List<GameObject> items = new List<GameObject>();
    [SerializeField] private List<ShelfController> shelves = new List<ShelfController> ();

    public override void OnStartServer()
    {
        base.OnStartServer();

        items.ForEach(item =>
        {
            int rndItemIdx;
            do
            {
                rndItemIdx = UnityEngine.Random.Range(0, shelves.Count);
            } while (!shelves[rndItemIdx].isEmptySpaceAvailable());

            shelves[rndItemIdx].setItem(item);
        });
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        // Register all prefabs
        foreach (var item in items)
        {
            NetworkClient.RegisterPrefab(item);
        }
    }

    public List<GameObject> GetItems()
    {
        return new List<GameObject>(items);
    }
    
}
