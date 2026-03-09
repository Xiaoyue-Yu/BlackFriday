using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    public bool isActive;

    private void Awake()
    {
        isActive = false;
    }

    private void Start()
    {
        PlayerShopInteraction.OnOpenShop += SetInactive;
        PlayerShopInteraction.OnCloseShop += SetActive;
        RunManager.Instance.OnRunStart += SetActive;
        RunManager.Instance.OnRunEnd += SetInactive;
    }


    private void OnDisable()
    {
        PlayerShopInteraction.OnOpenShop -= SetInactive;
        PlayerShopInteraction.OnCloseShop -= SetActive;
        RunManager.Instance.OnRunStart -= SetActive;
        RunManager.Instance.OnRunEnd -= SetInactive;
    }

    private void SetActive()
    {
        isActive = true;
    }
    
    private void SetActive(ShopArea obj)
    {
        isActive = true;
    }
    
    private void SetInactive()
    {
        isActive = false;
    }
    
    private void SetInactive(float earnings)
    {
        isActive = false;
    }
    
    private void SetInactive(ShopArea obj)
    {
        isActive = false;
    }

    private void Update()
    {
        if (!isActive) return;
        
        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, moveY);
        transform.position += move * moveSpeed * Time.deltaTime;
    }
}
