using UnityEngine;
using TMPro;
using System.Collections;

public class SimpleCheckout : MonoBehaviour
{
    public Transform customer;
    public Transform targetPosition;
    public float moveSpeed = 5f;

    public GameObject floatingTextPrefab;
    public Transform textSpawnPoint;
    public Canvas mainCanvas;
    public int itemPrice = 100;

    private bool isMoving = false;
    private bool hasCheckedOut = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hasCheckedOut)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.transform == targetPosition)
            {
                isMoving = true;
            }
        }

        if (isMoving)
        {
            customer.position = Vector3.Lerp(customer.position, targetPosition.position, Time.deltaTime * moveSpeed);

            if (Vector3.Distance(customer.position, targetPosition.position) < 0.1f)
            {
                customer.position = targetPosition.position;
                isMoving = false;
                hasCheckedOut = true;
                ShowFloatingText();
            }
        }
    }

    private void ShowFloatingText()
    {
        GameObject textObj = Instantiate(floatingTextPrefab, mainCanvas.transform);
        textObj.transform.position = Camera.main.WorldToScreenPoint(textSpawnPoint.position);

        TextMeshProUGUI floatingText = textObj.GetComponent<TextMeshProUGUI>();
        floatingText.text = "+$" + itemPrice.ToString();
        floatingText.color = Color.green;

        StartCoroutine(FloatAndFade(floatingText));
    }

    private IEnumerator FloatAndFade(TextMeshProUGUI textUI)
    {
        float elapsedTime = 0f;
        float floatDuration = 1.5f;
        Vector3 startPos = textUI.transform.position;
        Vector3 endPos = startPos + new Vector3(0, 150f, 0);
        Color startColor = textUI.color;

        while (elapsedTime < floatDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / floatDuration;

            textUI.transform.position = Vector3.Lerp(startPos, endPos, t);

            Color newColor = startColor;
            newColor.a = Mathf.Lerp(1f, 0f, t);
            textUI.color = newColor;

            yield return null;
        }

        Destroy(textUI.gameObject);
    }
}