using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingCart : MonoBehaviour
{
    [SerializeField] private Button shoppingCartButton;
    [SerializeField] private TextMeshProUGUI totalValueText;
    private Image sr;

    [SerializeField] private float animDuration = 0.8f;
    
    [SerializeField] private Sprite cartSpriteEmpty;
    [SerializeField] private Sprite cartSprite1;
    [SerializeField] private Sprite cartSprite2;
    [SerializeField] private Sprite cartSpriteMany;
    
    private Canvas canvas;

    private void Start()
    {
        ItemGO.OnAddToCart += ItemGOOnOnAddToCart;
        CartManager.Instance.OnCartCleared += InstanceOnOnCartCleared;
        
        canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("Cant find canvas");
        }

        sr = GetComponent<Image>();
        RefreshCartDisplay();
    }

    private void InstanceOnOnCartCleared()
    {
        RefreshCartDisplay();
    }

    private void OnDisable()
    {
        ItemGO.OnAddToCart -= ItemGOOnOnAddToCart;
        if (CartManager.Instance != null)
        {
            CartManager.Instance.OnCartCleared -= InstanceOnOnCartCleared;
        }
    }

    private void ItemGOOnOnAddToCart(GameObject obj)
    {
        RefreshCartValueText();

        var itemPrefab = obj;
        var targetTransform = shoppingCartButton.transform;
        var startPos = obj.transform.position;
        PlayBagAbsorbEffect(itemPrefab, startPos, targetTransform, canvas.transform, animDuration);
    }
    
    public void PlayBagAbsorbEffect(GameObject itemPrefab, Vector3 startPosition, Transform bagTarget, Transform canvasTransform, float duration = 1.0f)
    {
        // Spawn the item at the starting location
        GameObject spawnedItem = Instantiate(itemPrefab, startPosition, Quaternion.identity );
        spawnedItem.transform.SetParent(canvasTransform);
        spawnedItem.transform.SetAsLastSibling();

        // Create a DOTween sequence to play all animations at the same time
        Sequence absorbSequence = DOTween.Sequence();

        // 1. Move to the bag target
        absorbSequence.Join(spawnedItem.transform.DOMove(bagTarget.position, duration)
            .SetEase(Ease.InOutQuad));

        // 2. Spin rapidly (720 degrees on Z-axis)
        absorbSequence.Join(spawnedItem.transform.DORotate(new Vector3(0, 0, 720), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad));

        // 3. Shrink down to nothing, with a "sucking in" ease
        absorbSequence.Join(spawnedItem.transform.DOScale(Vector3.zero, duration)
            .SetEase(Ease.InBack));

        // 4. Fade out (Checks if it's a UI CanvasGroup or a 2D SpriteRenderer)
        if (spawnedItem.TryGetComponent<CanvasGroup>(out CanvasGroup canvasGroup))
        {
            absorbSequence.Join(canvasGroup.DOFade(0f, duration));
        }
        else if (spawnedItem.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
        {
            absorbSequence.Join(spriteRenderer.DOFade(0f, duration));
        }

        // 5. Destroy the item exactly when the sequence finishes
        absorbSequence.OnComplete(() =>
        {
            Destroy(spawnedItem);
            RefreshCartDisplay();
        });
    }

    private void RefreshCartDisplay()
    {
        UpdateCartSprite();
        RefreshCartValueText();
    }

    private void UpdateCartSprite()
    {
        var count = CartManager.Instance.curCart.Count;
        switch (count)
        {
            case 0:
                sr.sprite = cartSpriteEmpty;
                break;
            case 1:
                sr.sprite = cartSprite1;
                break;
            case 2:
                sr.sprite = cartSprite2;
                break;
            default:
                sr.sprite = cartSpriteMany;
                break;
        }
        Debug.Log("Updated Cart Sprite: " + count);
    }

    private void RefreshCartValueText()
    {
        if (totalValueText == null || CartManager.Instance == null) return;

        float totalValue = CartManager.Instance.GetCurrentCartTotalValue();
        totalValueText.text = "$" + totalValue.ToString("0.##");
    }
}
