using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckoutManager : MonoBehaviour
{
    public static CheckoutManager Instance;
    
    [SerializeField] private Button checkoutButton;

    public event Action OnCheckOutStart;
    public event Action OnCheckOutFinished;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (this.checkoutButton != null)
            {
                Instance.ApplyCheckoutButton(this.checkoutButton);
                
            }
            Destroy(gameObject);
            return;
        }
        
        if (checkoutButton != null)
        {
            ApplyCheckoutButton(checkoutButton);
            
        }

        
        
    }

    private void Start()
    {
        ItemGO.OnAddToCart += ItemGOOnOnAddToCart;
    }

    private void OnDisable()
    {
        ItemGO.OnAddToCart -= ItemGOOnOnAddToCart;
    }

    private void ItemGOOnOnAddToCart(GameObject obj)
    {
        RefreshButton();
    }

    public void ApplyCheckoutButton(Button button)
    {
        checkoutButton = button;
        checkoutButton.onClick.RemoveAllListeners();
        checkoutButton.onClick.AddListener(() => Checkout(CartManager.Instance.curCart));

        RefreshButton();
    }
    public void Checkout(List<ItemValue> cart)
    {
        Debug.Log("Checking out ...");
        OnCheckOutStart?.Invoke();
        
        float totalPrice = 0;
        foreach (var iv in cart)
        {
            var score = iv.CustomerScore;
            var price = iv.Price;
            RunManager.Instance.RunScore += score;
            totalPrice += price;
        }

        
        // TODO: apply score logic
        bool isBuying = ProcessCheckout(totalPrice, RunManager.Instance.RunScore);
        
        // add to run earnings
        if (isBuying)
        {
            RunManager.Instance.RunEarnings += totalPrice;
            RunManager.Instance.CustomerFulfilled += 1;
        }
        
        Debug.Log($"Customer {RunManager.Instance.curCustomerGO.customerData.customerName} bought: " + isBuying);

        // disable after checkout
        checkoutButton.enabled = false;
        
        OnCheckOutFinished?.Invoke();
    }

    private bool ProcessCheckout(float totalPrice, float totalScore)
    {
        var budget = RunManager.Instance.curCustomerGO.customerData.budget;

        if (budget >= totalPrice)
        {
            if (totalScore < 0)
            {
                return false;
            }
            else if (totalScore < 30)
            {
                if (UnityEngine.Random.value <= 0.2f) return true;
                return false;
            }
            else if (totalScore < 60)
            {
                if (UnityEngine.Random.value <= 0.5f) return true;
                return false;
            }
            else if (totalScore < 80)
            {
                if (UnityEngine.Random.value <= 0.95f) return true;
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            // twice the budget
            if (budget * 2 < totalPrice)
            {
                return false;
            }
            else
            {
                if (totalScore < 0)
                {
                    return false;
                }
                else if (totalScore < 20)
                {
                    if (UnityEngine.Random.value <= 0.2f) return true;
                    return false;
                }
                else if (totalScore < 40)
                {
                    if (UnityEngine.Random.value <= 0.3f) return true;
                    return false;
                }
                else if (totalScore < 70)
                {
                    if (UnityEngine.Random.value <= 0.5f) return true;
                    return false;
                }
                else if (totalScore < 80)
                {
                    if (UnityEngine.Random.value <= 0.7f) return true;
                    return false;
                }
                else if (totalScore < 100)
                {
                    if (UnityEngine.Random.value <= 0.85f) return true;
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
    private void RefreshButton()
    {
        // Debug.Log($"Refresh button: {CartManager.Instance.curCart}");
        checkoutButton.enabled = CartManager.Instance.curCart.Count > 0;
    }
}
