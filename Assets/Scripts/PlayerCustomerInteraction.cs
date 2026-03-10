using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerCustomerInteraction : MonoBehaviour
{
    private Collider2D customerInteractionTriggerCollider;

    public static event Action<CustomerGO> OnCustomerApproach;
    public static event Action<CustomerGO> OnCustomerAway;

    public static event Action<CustomerGO> OnCustomerServeStart;

    public CustomerGO availableCustomer = null;
    //public CustomerGO curCustomer = null;
    

    private void Awake()
    {
        if (customerInteractionTriggerCollider == null) customerInteractionTriggerCollider = GetComponent<Collider2D>();
        
    }

    private void Update()
    {
        if (RunManager.Instance.curCustomerGO == null && availableCustomer != null)
        {
            if (availableCustomer.isInteracted) return;
            if (Input.GetKey(KeyCode.E))
            {
                // serve customer
                RunManager.Instance.curCustomerGO = availableCustomer;
                availableCustomer = null;
                OnCustomerServeStart?.Invoke(RunManager.Instance.curCustomerGO);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (RunManager.Instance.curCustomerGO != null) return;
        if (other.CompareTag("Customer"))
        {
            var c = other.GetComponent<CustomerGO>();
            // make customer interactable
            OnCustomerApproach?.Invoke(c);
            availableCustomer = c;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (RunManager.Instance.curCustomerGO != null) return;
        if (other.CompareTag("Customer"))
        {
            var c = other.GetComponent<CustomerGO>();
            OnCustomerAway?.Invoke(c);
            availableCustomer = null;
        }
    }
}
