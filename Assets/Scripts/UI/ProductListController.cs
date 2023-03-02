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

    // Start is called before the first frame update
    void Start()
    {

        // For each child of productItemList, get the ProductItemController component
        foreach (Transform child in productItemsList.transform)
        {
            productItems.Add(child.GetComponent<ProductItemController>());
        }

    }

    public void setProducts(List<string> names)
    {

        if (names.Count != productCount)
        {
            Debug.LogError("ProductListController: setProducts: names count is not equal to productCount");
            return;
        }

        for (int i = 0; i < productCount; i++)
        {
            productItems[i].setText(names[i]);
            productItems[i].setOutOfStock(false);
            productItems[i].setChecked(false);
        }
    }

    public void updateProductStock(int productIndex, bool isOutOfStock)
    {
        productItems[productIndex].setOutOfStock(isOutOfStock);
    }

    public void replaceProduct(int index, string name, bool isOutOfStock = false)
    {
        productItems[index].setText(name);
        productItems[index].setOutOfStock(false);
        productItems[index].setChecked(isOutOfStock);
    }

    private IEnumerator delayedProductReplacement(int index, string name)
    {
        yield return new WaitForSeconds(checkAnimationDuration);
        replaceProduct(index, name);
    }

    public void checkAndReplaceProduct(int index, string nextProduct)
    {
        productItems[index].setChecked(true);
        StartCoroutine(delayedProductReplacement(index, nextProduct));
    }

}
