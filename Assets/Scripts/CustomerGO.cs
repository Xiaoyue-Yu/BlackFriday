using System;
using UnityEngine;

public class CustomerGO : MonoBehaviour
{
    public Customer customerData;

    public bool isInteracted = false;
    // public bool isInteractable { get; private set; } = false;

    public static event Action<string> OnCustomerServeStart;
    

    private void Start()
    {
        PlayerCustomerInteraction.OnCustomerApproach += PlayerCustomerInteractionOnOnCustomerApproach;
        PlayerCustomerInteraction.OnCustomerAway += PlayerCustomerInteractionOnOnCustomerAway;
        PlayerCustomerInteraction.OnCustomerServeStart += PlayerCustomerInteractionOnOnCustomerServeStart;
        // CheckoutManager.Instance.OnCheckOutFinished += InstanceOnOnCheckOutFinished;
        if (CheckoutManager.Instance != null)
        {
            CheckoutManager.Instance.OnCheckOutFinished += InstanceOnOnCheckOutFinished;
        }
    }

    private void InstanceOnOnCheckOutFinished()
    {
        isInteracted = true;
    }

    private void PlayerCustomerInteractionOnOnCustomerServeStart(CustomerGO obj)
    {
        if (obj != this) return;
        // TODO: hide "E"
        // Start Serve 
        ServeCustomer();
    }

    private void ServeCustomer()
    {
        Debug.Log("Customer interaction triggered: " + customerData.customerId);
        OnCustomerServeStart?.Invoke(this.customerData.customerId);
        // TODO: start dialogue?
        
        
        
    }

    private void OnDisable()
    {
        PlayerCustomerInteraction.OnCustomerApproach -= PlayerCustomerInteractionOnOnCustomerApproach;
        PlayerCustomerInteraction.OnCustomerAway -= PlayerCustomerInteractionOnOnCustomerAway;
        PlayerCustomerInteraction.OnCustomerServeStart -= PlayerCustomerInteractionOnOnCustomerServeStart;
        // CheckoutManager.Instance.OnCheckOutFinished -= InstanceOnOnCheckOutFinished;
        if (CheckoutManager.Instance != null)
        {
            CheckoutManager.Instance.OnCheckOutFinished -= InstanceOnOnCheckOutFinished;
        }
    }

    private void PlayerCustomerInteractionOnOnCustomerAway(CustomerGO obj)
    {
        if (obj != this) return;
        if (isInteracted) return;
        // TODO: hide "E"
    }

    private void PlayerCustomerInteractionOnOnCustomerApproach(CustomerGO obj)
    {
        if (obj != this) return;
        if (isInteracted) return;
        // TODO: show "E" to interact
    }
}
