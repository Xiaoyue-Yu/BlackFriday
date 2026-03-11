using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CustomerGO : MonoBehaviour
{
    private static readonly List<CustomerGO> AllCustomers = new List<CustomerGO>();
    private static CustomerGO activeCustomer;
    private static int nextCustomerIndex;
    private static bool initialSpawnScheduled;

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
    [SerializeField] private float nextCustomerDelay = 0.5f;

    private float currentFloatY;
    private SpriteRenderer popupSR;
    private Tween floatTween;
    private SpriteRenderer customerSpriteRenderer;
    private Collider2D customerCollider;
    private Animator customerAnimator;
    private CustomerWander customerWander;

    public static event Action<string> OnCustomerServeStart;

    private void Awake()
    {
        if (!AllCustomers.Contains(this))
        {
            AllCustomers.Add(this);
        }

        customerSpriteRenderer = GetComponent<SpriteRenderer>();
        customerCollider = GetComponent<Collider2D>();
        customerAnimator = GetComponent<Animator>();
        customerWander = GetComponent<CustomerWander>();

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

        ResetPresentation();
        SetCustomerVisible(false);

        if (!initialSpawnScheduled)
        {
            SortCustomersById();
            initialSpawnScheduled = true;
            StartCoroutine(SpawnInitialCustomerNextFrame());
        }
    }

    private IEnumerator SpawnInitialCustomerNextFrame()
    {
        yield return null;
        SpawnNextCustomerInSequence();
    }

    private void LateUpdate()
    {
        if (popupInstance != null)
        {
            Vector3 finalOffset = popupOffset + new Vector3(0, currentFloatY, 0);
            popupInstance.transform.position = transform.position + finalOffset;
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
        if (popupInstance != null)
        {
            popupInstance.transform.localScale = poofPopupScale;
        }
        PlayPoof(isBuying ? buySuccess : buyFail);
    }

    private void PlayerCustomerInteractionOnOnCustomerServeStart(CustomerGO obj)
    {
        if (obj != this) return;
        DeHighlight();
        ServeCustomer();
    }

    private void ServeCustomer()
    {
        OnCustomerServeStart?.Invoke(customerData.customerId);
    }

    private void OnDisable()
    {
        PlayerCustomerInteraction.OnCustomerApproach -= PlayerCustomerInteractionOnOnCustomerApproach;
        PlayerCustomerInteraction.OnCustomerAway -= PlayerCustomerInteractionOnOnCustomerAway;
        PlayerCustomerInteraction.OnCustomerServeStart -= PlayerCustomerInteractionOnOnCustomerServeStart;

        if (CheckoutManager.Instance != null)
        {
            CheckoutManager.Instance.OnCheckOutFinished -= InstanceOnOnCheckOutFinished;
        }
    }

    private void PlayerCustomerInteractionOnOnCustomerAway(CustomerGO obj)
    {
        if (obj != this) return;
        if (isInteracted) return;
        DeHighlight();
    }

    private void PlayerCustomerInteractionOnOnCustomerApproach(CustomerGO obj)
    {
        if (obj != this) return;
        if (isInteracted) return;
        Highlight();
    }

    private void Highlight()
    {
        if (popupSR == null) return;

        popupSR.sprite = interactSprite;
        popupSR.color = Color.white;
        popupInstance.transform.localScale = interactionPopupScale;
        StartFloating();
    }

    private void DeHighlight()
    {
        if (popupSR == null) return;

        popupSR.sprite = null;
        popupSR.color = Color.white;
        popupInstance.transform.localScale = popupScale;
        StopFloating(true);
    }

    private void StartFloating()
    {
        floatTween?.Kill();
        currentFloatY = 0f;

        floatTween = DOTween.To(() => currentFloatY, x => currentFloatY = x, floatDistance, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void StopFloating(bool returnToStart = true)
    {
        floatTween?.Kill();
        if (returnToStart)
        {
            DOTween.To(() => currentFloatY, x => currentFloatY = x, 0f, 0.5f).SetEase(Ease.OutQuad);
        }
    }

    [SerializeField] private float poofDistance = 1.5f;
    [SerializeField] private float poofDuration = 0.6f;

    public void PlayPoof(Sprite statusSprite)
    {
        if (popupSR == null) return;

        floatTween?.Kill();
        popupSR.sprite = statusSprite;
        popupSR.color = Color.white;

        Sequence poofSeq = DOTween.Sequence();
        poofSeq.Append(
            DOTween.To(() => currentFloatY, x => currentFloatY = x, poofDistance, poofDuration)
                .SetEase(Ease.OutBack)
        );
        poofSeq.Join(popupSR.DOFade(0, poofDuration).SetEase(Ease.InQuad));
        poofSeq.OnComplete(() =>
        {
            popupSR.sprite = null;
            currentFloatY = 0f;
        });
    }

    public void PrepareForVisit()
    {
        isInteracted = false;
        ResetPresentation();
        SetCustomerVisible(true);

        if (customerWander != null)
        {
            customerWander.BeginEntry();
        }
    }

    public void FinishVisitAndQueueNext()
    {
        SetCustomerVisible(false);

        if (activeCustomer == this)
        {
            activeCustomer = null;
        }

        StartCoroutine(SpawnNextCustomerAfterDelay());
    }

    private IEnumerator SpawnNextCustomerAfterDelay()
    {
        yield return new WaitForSeconds(nextCustomerDelay);
        SpawnNextCustomerInSequence();
    }

    private void ResetPresentation()
    {
        StopFloating(false);
        currentFloatY = 0f;

        if (popupSR != null)
        {
            popupSR.DOKill();
            popupSR.sprite = null;
            popupSR.color = Color.white;
        }

        if (popupInstance != null)
        {
            popupInstance.transform.localScale = popupScale;
        }
    }

    private void SetCustomerVisible(bool isVisible)
    {
        if (customerSpriteRenderer != null)
        {
            customerSpriteRenderer.enabled = isVisible;
        }

        if (customerCollider != null)
        {
            customerCollider.enabled = isVisible;
        }

        if (customerAnimator != null)
        {
            customerAnimator.enabled = isVisible;
        }

        if (popupInstance != null)
        {
            popupInstance.SetActive(isVisible);
        }

        if (customerWander != null)
        {
            customerWander.enabled = isVisible;
        }
    }

    private static void SortCustomersById()
    {
        AllCustomers.Sort((a, b) => string.CompareOrdinal(a.customerData.customerId, b.customerData.customerId));
    }

    private static void SpawnNextCustomerInSequence()
    {
        if (nextCustomerIndex >= AllCustomers.Count)
        {
            if (RunManager.Instance != null)
            {
                RunManager.Instance.EndRun();
            }
            return;
        }

        CustomerGO nextCustomer = AllCustomers[nextCustomerIndex];
        nextCustomerIndex += 1;

        if (nextCustomer == null)
        {
            SpawnNextCustomerInSequence();
            return;
        }

        activeCustomer = nextCustomer;
        nextCustomer.PrepareForVisit();
    }

    private void OnDestroy()
    {
        AllCustomers.Remove(this);
        if (activeCustomer == this)
        {
            activeCustomer = null;
        }
        if (AllCustomers.Count == 0)
        {
            initialSpawnScheduled = false;
            nextCustomerIndex = 0;
        }

        if (popupInstance != null)
        {
            Destroy(popupInstance);
        }
    }
}
