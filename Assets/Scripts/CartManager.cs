using System;
using System.Collections.Generic;
using UnityEngine;

// customer based
public class CartManager : MonoBehaviour
{
    public static CartManager Instance;
    
    public List<ItemValue> curCart = new List<ItemValue>();
    public Dictionary<string, List<ItemValue>> cartsThisRun = new Dictionary<string, List<ItemValue>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        RunManager.Instance.OnRunStart += InstanceOnOnRunStart;
        CustomerGO.OnCustomerServeStart += CustomerGOOnOnCustomerServeStart;
    }

    private void OnDisable()
    {
        RunManager.Instance.OnRunStart -= InstanceOnOnRunStart;
        CustomerGO.OnCustomerServeStart -= CustomerGOOnOnCustomerServeStart;
    }

    private void InstanceOnOnRunStart()
    {
        cartsThisRun.Clear();
        curCart.Clear();
    }

    private void CustomerGOOnOnCustomerServeStart(string customerId)
    {
        // Check if we already have a cart for this customer
        if (cartsThisRun.ContainsKey(customerId))
        {
            // Load the existing cart into curCart
            curCart = cartsThisRun[customerId];
        }
        else
        {
            List<ItemValue> newCart = new List<ItemValue>();
            cartsThisRun.Add(customerId, newCart);
            curCart = newCart;
        }
    }
}
