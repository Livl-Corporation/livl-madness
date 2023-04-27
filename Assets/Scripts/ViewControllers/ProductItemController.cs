using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductItemController
    : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite checkedSprite;
    [SerializeField] private Sprite uncheckedSprite;

    [Header("Components")]
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text outOfStock;
    [SerializeField] private GameObject checkbox;

    private Image checkboxImage;

    public void Awake()
    {
        checkboxImage = checkbox.GetComponent<Image>();
    }

    public void SetText(String text)
    {
        title.text = text;
    }

    public void SetOutOfStock(bool isOutOfStock)
    {
        outOfStock.gameObject.SetActive(isOutOfStock);
    }

    public void SetChecked(bool isChecked)
    {
        checkboxImage.sprite = isChecked ? checkedSprite : uncheckedSprite;
    }

}
