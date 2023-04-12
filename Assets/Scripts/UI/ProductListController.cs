using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductListController : MonoBehaviour
{

    [Header("Config")]
    [SerializeField] private int checkAnimationDuration = 2;

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

    // Start is called before the first frame update
    public void SetProducts(List<string> names)
    {
        for (int i = 0; i < productCount; i++)
        {
            ReplaceProduct(i, names.Count > i ? names[i] : "");
        }
    }

    public void UpdateProductStock(int productIndex, bool isOutOfStock)
    {
        productItems[productIndex].SetOutOfStock(isOutOfStock);
    }

    public void ReplaceProduct(int index, string name, bool isOutOfStock = false)
    {
        productItems[index].SetText(name);
        productItems[index].SetOutOfStock(false);
        productItems[index].SetChecked(isOutOfStock);
    }

    private IEnumerator DelayedProductReplacement(int index, string name)
    {
        yield return new WaitForSeconds(checkAnimationDuration);
        ReplaceProduct(index, name);
    }

    public void CheckAndReplace(int index, List<string> newList)
    {
        productItems[index].SetChecked(true);
        
        // Replace product after index & before productCount
        for (int i = index + 1; i < productCount; i++)
        { 
            StartCoroutine(DelayedProductReplacement(index, newList[index]));
        }
        
    }

}
