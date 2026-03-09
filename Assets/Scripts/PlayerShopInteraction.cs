using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerShopInteraction : MonoBehaviour
{
    [SerializeField] private bool isViewingShop = false;
    [SerializeField] private ShopArea activeShopArea;

    public static event Action<ShopArea> OnEnterShop;         // Enter perimeter
    public static event Action<ShopArea> OnLeaveShop;
    public static event Action<ShopArea> OnOpenShop;
    public static event Action<ShopArea> OnCloseShop;
    
    // private PlayerMovement _playerMovement;
    

    private void Update()
    {
        if (activeShopArea == null) return;
        if (Input.GetKey(KeyCode.Space))
        {
            OnOpenShop?.Invoke(activeShopArea);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            OnCloseShop?.Invoke(activeShopArea);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("ShopArea"))
        {
            if (activeShopArea != null)
            {
                Debug.Log($"Invoke leave from prev {activeShopArea}");
                OnLeaveShop?.Invoke(activeShopArea);
            }
            activeShopArea = other.GetComponent<ShopArea>();
            OnEnterShop?.Invoke(activeShopArea);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ShopArea"))
        {
            OnLeaveShop?.Invoke(other.GetComponent<ShopArea>());
        }
    }
}
