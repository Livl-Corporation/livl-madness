using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class StoreItemsController : MonoBehaviour
{
    [SerializeField] private List<GameObject> items = new List<GameObject>();
    [SerializeField] private List<ShelfController> shelves = new List<ShelfController> ();

    void Start()
    {
        //    Debug.Log("STARTING STORE ITEM CONTROLLER");
        //    this.loadItems();
        //    Debug.Log("ITEMS LOADED COUNT : " + this.items.Count);

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
