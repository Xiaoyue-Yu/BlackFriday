using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ShopArea : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject panel;

    [SerializeField] private bool isTriggered = false;
    [SerializeField] private bool isPanelActivated = false;

    private Collider2D colZone;
    private SpriteRenderer sr;

    public bool ShouldLockPlayerMovement
    {
        get
        {
            if (panel == null) return false;
            return panel.GetComponent<ClosetSelection>() != null;
        }
    }

    private void Awake()
    {
        if (colZone == null)
        {
            colZone = GetComponent<Collider2D>();
        }

        colZone.isTrigger = true;
        sr = GetComponent<SpriteRenderer>();

        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    private void Start()
    {
        PlayerShopInteraction.OnOpenShop += PlayerShopInteractionOnOnOpenShop;
        PlayerShopInteraction.OnCloseShop += PlayerShopInteractionOnOnCloseShop;
        PlayerShopInteraction.OnEnterShop += PlayerShopInteractionOnOnEnterShop;
        PlayerShopInteraction.OnLeaveShop += PlayerShopInteractionOnOnLeaveShop;
        CheckoutManager.Instance.OnCheckOutStart += () =>
        {
            if (panel)
            {
                ClosetSelection closet = panel.GetComponent<ClosetSelection>();
                if (closet != null)
                {
                    closet.CloseCloset();
                }
                else
                {
                    panel.SetActive(false);
                }
            }

            isPanelActivated = false;
        };
    }

    private void OnDisable()
    {
        PlayerShopInteraction.OnOpenShop -= PlayerShopInteractionOnOnOpenShop;
        PlayerShopInteraction.OnCloseShop -= PlayerShopInteractionOnOnCloseShop;
        PlayerShopInteraction.OnEnterShop -= PlayerShopInteractionOnOnEnterShop;
        PlayerShopInteraction.OnLeaveShop -= PlayerShopInteractionOnOnLeaveShop;
    }

    private void PlayerShopInteractionOnOnLeaveShop(ShopArea shopArea)
    {
        Debug.Log($"Leaving {shopArea}");
        if (shopArea != this) return;
        Dehighlight();
    }

    private void PlayerShopInteractionOnOnEnterShop(ShopArea shopArea)
    {
        if (shopArea != this) return;
        Highlight();
    }

    private void PlayerShopInteractionOnOnCloseShop(ShopArea shopArea)
    {
        if (shopArea != this) return;

        if (panel)
        {
            ClosetSelection closet = panel.GetComponent<ClosetSelection>();
            if (closet != null)
            {
                closet.CloseCloset();
            }
            else
            {
                panel.SetActive(false);
            }
        }
        isPanelActivated = false;
    }

    private void PlayerShopInteractionOnOnOpenShop(ShopArea shopArea)
    {
        if (shopArea != this) return;

        if (panel)
        {
            ClosetSelection closet = panel.GetComponent<ClosetSelection>();
            if (closet != null)
            {
                panel.SetActive(true);
                closet.OpenCloset();
            }
        }
        isPanelActivated = true;
    }

    private void Highlight()
    {
        sr.color = Color.yellow;
    }

    private void Dehighlight()
    {
        sr.color = Color.white;
    }
}
