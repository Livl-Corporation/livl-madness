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

    private void Awake()
    {
        // For each child of productItemList, get the ProductItemController component
        foreach (Transform child in productItemsList.transform)
        {
            productItems.Add(child.GetComponent<ProductItemController>());
        }
        
        Debug.Log("ProductListController: " + productItems.Count + " product items found");
    }

    public void UpdateProductList(List<ProductItem> productList)
    {
        for (int i = 0; i < productCount; i++)
        {
            ReplaceProduct(i, productList.Count > i ? productList[i] : null);
        }
    }
    
    private void ReplaceProduct(int index, ProductItem item)
    {
        if(index >= productItems.Count || index < 0)
        {
            Debug.LogError("ProductListController: Index out of range");
            return;
        }
        
        var productItem = productItems[index];

        productItem.SetText(item.name ?? "");
        productItem.SetOutOfStock(item.isOutOfStock);
        productItem.SetChecked(item.scanned);
    }

}