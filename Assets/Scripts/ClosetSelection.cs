using UnityEngine;
using UnityEngine.UI;

public class ClosetSelection : MonoBehaviour
{
    public RectTransform[] clothesSlots;
    public RectTransform selectionFrame;
    public GameObject closetPanel;

    private int currentIndex = 0;
    private bool isClosetOpen = false;

    void Update()
    {
        if (!isClosetOpen) return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = clothesSlots.Length - 1;
            UpdateFramePosition();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentIndex++;
            if (currentIndex >= clothesSlots.Length) currentIndex = 0;
            UpdateFramePosition();
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmSelection();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCloset();
        }
    }

    public void OpenCloset()
    {
        closetPanel.SetActive(true);
        isClosetOpen = true;
        currentIndex = 0;
        UpdateFramePosition();
    }

    public void CloseCloset()
    {
        closetPanel.SetActive(false);
        isClosetOpen = false;
    }

    private void UpdateFramePosition()
    {
        if (clothesSlots.Length > 0 && selectionFrame != null)
        {
            selectionFrame.position = clothesSlots[currentIndex].position;
        }
    }

    private void ConfirmSelection()
    {
        Debug.Log("Player confirmed item index: " + currentIndex);
        CloseCloset();
    }
}