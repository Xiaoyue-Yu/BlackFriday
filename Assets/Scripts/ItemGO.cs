using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemGO : MonoBehaviour
{
    [Header("Refs")] 
    public Item itemData;

    public int quantity;

    [SerializeField] private Button _addToCartButton;
    [SerializeField] private TextMeshProUGUI remainingQuantityText;
    [SerializeField] private TextMeshProUGUI priceTagText;

    public static event Action<Item> OnAddToCart;

    private Image img;
    private void Awake()
    {
        if (itemData == null)
        {
            Debug.LogWarning("NO Item data!");
            return;
        }
        quantity = itemData.quantityInStock;
        priceTagText.text = itemData.price.ToString();
        
        if (_addToCartButton == null)
        {
            _addToCartButton = GetComponentInChildren<Button>();
            Debug.LogWarning("NO Button!");
            return;
        }
        _addToCartButton.onClick.AddListener(AddToCart);

        img = GetComponentsInChildren<Image>()[0];
        img.sprite = itemData.itemSprite;
        
        RefreshUI();
    }

    public void AddToCart()
    {
        // Cannot add when serving noone
        if (RunManager.Instance.curCustomerGO == null) return;
        
        CartManager.Instance.curCart.Add(this.itemData.ToItemValue(RunManager.Instance.curCustomerGO.customerData.customerId));
        quantity -= 1;
        Debug.Log("Added, Cart count: " + CartManager.Instance.curCart.Count);
        RefreshUI();
        OnAddToCart?.Invoke(this.itemData);
    }

    private void RefreshUI()
    {
        if (_addToCartButton != null)
        {
            _addToCartButton.enabled = quantity > 0;
        }

        if (remainingQuantityText != null)
        {
            remainingQuantityText.text = $"Stock: {quantity}";
        }
    }
}
