using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductItemController
    : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite checkedSprite;
    [SerializeField] private Sprite uncheckedSprite;
    [SerializeField] private Sprite failSprite;

    [Header("Components")]
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text outOfStock;
    [SerializeField] private GameObject checkbox;

    private Image checkboxImage;
    private Dictionary<ProductState, Sprite> stateToSprite;
    
    public void Awake()
    {
        checkboxImage = checkbox.GetComponent<Image>();
        stateToSprite = new Dictionary<ProductState, Sprite>()
        {
            {ProductState.FAILED, failSprite },
            {ProductState.CHECKED, checkedSprite },
            {ProductState.UNCHECKED, uncheckedSprite }
        };
    }

    public void SetProduct(ProductItem item)
    {
        var hasItem = item != null;
        
        SetText(hasItem ? item.name :  "---");
        SetOutOfStock(hasItem && item.isOutOfStock);
        SetState(hasItem ? item.status : ProductState.UNCHECKED);
    }

    private void SetText(string text)
    {
        title.text = text;
    }

    private void SetOutOfStock(bool isOutOfStock)
    {
        outOfStock.gameObject.SetActive(isOutOfStock);
    }

    private void SetState(ProductState state)
    {
        checkboxImage.sprite = stateToSprite[state];
    }

}
