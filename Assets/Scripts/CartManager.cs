using System;
using System.Collections.Generic;
using UnityEngine;

// customer based
public class CartManager : MonoBehaviour
{
    public static CartManager Instance;
    
    public List<ItemValue> curCart = new List<ItemValue>();
    public Dictionary<string, List<ItemValue>> cartsThisRun = new Dictionary<string, List<ItemValue>>();
    [SerializeField] private string curCustomerId;

    public event Action OnCartCleared;

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
        CheckoutManager.Instance.OnCheckOutFinished += InstanceOnOnCheckOutFinished;
    }

    private void InstanceOnOnCheckOutFinished(bool isBuying)
    {
        cartsThisRun.Add(curCustomerId, new List<ItemValue>(curCart));
        Debug.Log("Save to run: " + cartsThisRun[curCustomerId].Count);
        curCustomerId = "";
        ClearCart();
    }
    

    private void OnDisable()
    {
        RunManager.Instance.OnRunStart -= InstanceOnOnRunStart;
        CustomerGO.OnCustomerServeStart -= CustomerGOOnOnCustomerServeStart;
        CheckoutManager.Instance.OnCheckOutFinished -= InstanceOnOnCheckOutFinished;
    }

    private void InstanceOnOnRunStart()
    {
        cartsThisRun = new Dictionary<string, List<ItemValue>>();
        ClearCart();
    }

    private void CustomerGOOnOnCustomerServeStart(string customerId)
    {
        curCustomerId = customerId;
        ClearCart();
    }

    private void ClearCart()
    {
        curCart.Clear();
        OnCartCleared?.Invoke();
        Debug.Log("Cleared cart");
    }
}
