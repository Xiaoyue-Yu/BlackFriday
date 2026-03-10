using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CustomerGO : MonoBehaviour
{
    public Customer customerData;

    public bool isInteracted = false;
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private Sprite interactSprite;
    [SerializeField] private Sprite buySuccess;
    [SerializeField] private Sprite buyFail;

    private GameObject popupInstance;
    [SerializeField] private Vector3 popupOffset = new Vector3(0, 1f, 0);
    [SerializeField] private Vector3 popupScale = Vector3.one;
    [SerializeField] private Vector3 poofPopupScale = new Vector3(0.2f, 0.2f);
    [SerializeField] private Vector3 interactionPopupScale = new Vector3(0.2f, 0.2f);
    [SerializeField] private float floatDistance = 0.5f;
    [SerializeField] private float floatDuration = 1f;
    //[SerializeField] private float origLocalY;
    private float _currentFloatY;
    private SpriteRenderer popupSR;
    private Tween floatTween;

    public static event Action<string> OnCustomerServeStart;

    private void Awake()
    {
        if (popupPrefab == null)
        {
            Debug.LogWarning("No popup");
            return;
        }
        
        popupInstance = Instantiate(popupPrefab, transform.position + popupOffset, Quaternion.identity);
        
        popupSR = popupInstance.GetComponent<SpriteRenderer>();
        popupSR.sprite = null;
        
    }


    private void Start()
    {
        PlayerCustomerInteraction.OnCustomerApproach += PlayerCustomerInteractionOnOnCustomerApproach;
        PlayerCustomerInteraction.OnCustomerAway += PlayerCustomerInteractionOnOnCustomerAway;
        PlayerCustomerInteraction.OnCustomerServeStart += PlayerCustomerInteractionOnOnCustomerServeStart;
        CheckoutManager.Instance.OnCheckOutFinished += InstanceOnOnCheckOutFinished;
        
    }
    
    private void LateUpdate()
    {
        if (popupInstance != null)
        {
            // Position = Customer + Base Offset + Current Bobbing Amount
            Vector3 finalOffset = popupOffset + new Vector3(0, _currentFloatY, 0);
            popupInstance.transform.position = transform.position + finalOffset;
        
            // Ensure rotation stays zeroed
            popupInstance.transform.rotation = Quaternion.identity;
        }
    }

    private void InstanceOnOnCheckOutFinished(bool isBuying)
    {
        isInteracted = true;
        StartCoroutine(PostCheckout(isBuying));
    }

    private IEnumerator PostCheckout(bool isBuying)
    {
        yield return new WaitForSeconds(6f);
        popupInstance.transform.localScale = poofPopupScale;
        PlayPoof(isBuying ? buySuccess : buyFail);
    }

    private void PlayerCustomerInteractionOnOnCustomerServeStart(CustomerGO obj)
    {
        if (obj != this) return;
        // TODO: hide "E"
        DeHighlight();
        // Start Serve 
        ServeCustomer();
    }

    private void ServeCustomer()
    {
        OnCustomerServeStart?.Invoke(this.customerData.customerId);
        // TODO: start dialogue?
        
        
        
    }

    private void OnDisable()
    {
        PlayerCustomerInteraction.OnCustomerApproach -= PlayerCustomerInteractionOnOnCustomerApproach;
        PlayerCustomerInteraction.OnCustomerAway -= PlayerCustomerInteractionOnOnCustomerAway;
        PlayerCustomerInteraction.OnCustomerServeStart -= PlayerCustomerInteractionOnOnCustomerServeStart;
        CheckoutManager.Instance.OnCheckOutFinished -= InstanceOnOnCheckOutFinished;
    }

    private void PlayerCustomerInteractionOnOnCustomerAway(CustomerGO obj)
    {
        if (obj != this) return;
        if (isInteracted) return;
        // TODO: hide "E"
        DeHighlight();
    }

    private void PlayerCustomerInteractionOnOnCustomerApproach(CustomerGO obj)
    {
        if (obj != this) return;
        if (isInteracted) return;
        // TODO: show "E" to interact
        Highlight();
        
    }

    private void Highlight()
    {
        popupSR.sprite = interactSprite;
        popupInstance.transform.localScale = interactionPopupScale;
        StartFloating();
    }

    private void DeHighlight()
    {
        popupSR.sprite = null;
        popupInstance.transform.localScale = popupScale;
        StopFloating(true);
    }

    private void StartFloating()
    {
        floatTween?.Kill();
        // Reset the internal float tracker
        _currentFloatY = 0; 
    
        // Tween a simple float value rather than the Transform directly
        floatTween = DOTween.To(() => _currentFloatY, x => _currentFloatY = x, floatDistance, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
    
    public void StopFloating(bool returnToStart = true)
    {
        floatTween?.Kill();
        if (returnToStart)
        {
            DOTween.To(() => _currentFloatY, x => _currentFloatY = x, 0f, 0.5f).SetEase(Ease.OutQuad);
        }
    }
    
    [SerializeField] private float poofDistance = 1.5f;
    [SerializeField] private float poofDuration = 0.6f;

    public void PlayPoof(Sprite statusSprite)
    {
        // 1. Stop any current floating
        floatTween?.Kill();
    
        // 2. Set the sprite (Success or Fail)
        popupSR.sprite = statusSprite;
        popupSR.color = Color.white; // Ensure it's fully visible at start

        // 3. Create a Sequence
        Sequence poofSeq = DOTween.Sequence();

        poofSeq.Append(
            // Move up quickly
            DOTween.To(() => _currentFloatY, x => _currentFloatY = x, poofDistance, poofDuration)
                .SetEase(Ease.OutBack) // "OutBack" gives it a nice little pop/overshoot
        );

        poofSeq.Join(
            // Fade out at the same time (or slightly delayed)
            popupSR.DOFade(0, poofDuration).SetEase(Ease.InQuad)
        );

        // 4. Cleanup when done
        poofSeq.OnComplete(() => {
            popupSR.sprite = null;
            _currentFloatY = 0; // Reset for the next time it bobs
        });
    }
    
    private void OnDestroy()
    {
        // If the customer is deleted, take the popup with it
        if (popupInstance != null)
        {
            Destroy(popupInstance);
        }
    }
    
}
