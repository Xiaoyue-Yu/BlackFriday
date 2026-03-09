using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    public bool isActive;

    private Animator animator;

    private void Awake()
    {
        isActive = true;
        animator = GetComponent<Animator>();
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

    private void SetActive() { isActive = true; }
    private void SetActive(ShopArea obj) { isActive = true; }

    private void SetInactive()
    {
        isActive = false;
        if (animator != null) animator.SetBool("isMoving", false);
    }

    private void SetInactive(float earnings)
    {
        isActive = false;
        if (animator != null) animator.SetBool("isMoving", false);
    }

    private void SetInactive(ShopArea obj)
    {
        isActive = false;
        if (animator != null) animator.SetBool("isMoving", false);
    }

    private void Update()
    {
        if (!isActive) return;

        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, moveY);

        if (move.magnitude > 1f) move.Normalize();

        transform.position += move * moveSpeed * Time.deltaTime;

        bool isMoving = move.sqrMagnitude > 0.01f;
        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
        }

        if (move != Vector3.zero)
        {
            transform.up = move;
        }
    }
}