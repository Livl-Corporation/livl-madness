using System.Collections.Generic;
using UnityEngine;

public class ProductsController : MonoBehaviour
{
    [SerializeField] private List<GameObject> items = new List<GameObject>();
    [SerializeField] private List<ShelfController> shelves = new List<ShelfController> ();

    private void Start()
    {

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
    
    public List<GameObject> GetItems()
    {
        return new List<GameObject>(items);
    }
    
}
