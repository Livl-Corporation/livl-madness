using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Models;
using UnityEngine;

public class ProductListController : MonoBehaviour, IProductListObserver
{

    [Header("Components")]
    [SerializeField] private int productCount = 4;
    [SerializeField] private GameObject productItemsList;

    private List<ProductItemController> productItems = new List<ProductItemController>();

    private string playerName;
    
    private void Awake()
    {
        // For each child of productItemList, get the ProductItemController component
        foreach (Transform child in productItemsList.transform)
        {
            productItems.Add(child.GetComponent<ProductItemController>());
        }
        
        Debug.Log("ProductListController: " + productItems.Count + " product items found");
    }

    private void Start()
    {
        // Find the player name
        playerName = FindObjectOfType<Player>().name;
    }

    public void UpdateProductList(List<ProductItem> productList)
    {
        for (int i = 0; i < productCount; i++)
        {
            
            if (i >= productList.Count)
            {
                ReplaceProduct(i, null);
                continue;
            }

            var item = productList[i];

            if (!item.Scanned)
            {
                ReplaceProduct(i, new DisplayedProductItem(item));
                continue;
            }
            
            ReplaceProduct(i, new DisplayedProductItem(item, item.ScannedBy == playerName));

        }
    }
    
    private void ReplaceProduct(int index, DisplayedProductItem item)
    {
        if(index >= productItems.Count || index < 0)
        {
            Debug.LogError("ProductListController: Index out of range");
            return;
        }
        
        productItems[index].SetProduct(item);
 
    }

}
