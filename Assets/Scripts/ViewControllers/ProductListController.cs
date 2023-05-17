using System.Collections.Generic;
using Interfaces;
using Models;
using UnityEngine;

public class ProductListController : MonoBehaviour, IProductListObserver
{

    [Header("Components")]
    [SerializeField] private int productCount = 4;
    [SerializeField] private GameObject productItemsList;
    [SerializeField] private ScanListController scanListController;

    private readonly List<ProductItemController> productItems = new List<ProductItemController>();

    private AudioSource audioSource;
    
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
        scanListController = FindObjectOfType<ScanListController>();
        if (scanListController == null)
        {
            Debug.LogError("ProductListController: ScanListController not found");
            return;
        }
        scanListController.AddObserver(this);
        
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
        {
            Debug.LogError("ProductListController: AudioSource not found");
        }
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


            var isScannedByLocalPlayer = item.ScannedBy == Player.LocalPlayerName;
            ReplaceProduct(i, new DisplayedProductItem(item, isScannedByLocalPlayer));

            if (!isScannedByLocalPlayer)
            {
                audioSource.Play();
            }

        }
    }
    
    private void ReplaceProduct(int index, DisplayedProductItem item)
    {
        if(index >= productItems.Count || index < 0)
        {
            Debug.LogError("ProductListController: Index out of range");
            return;
        }
        
        if (item == null)
        {
            productItems[index].SetProduct(null);
            return;
        }
        
        productItems[index].SetProduct(item);
 
    }

    private void OnDestroy()
    {
        if (scanListController == null)
            return;
        
        scanListController.RemoveObserver(this);
    }
}
