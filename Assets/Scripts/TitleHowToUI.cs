using UnityEngine;
using UnityEngine.UI;

public class TitleHowToUI : MonoBehaviour
{
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject howToPanel;

    private void Start()
    {
        if (howToPanel != null)
        {
            howToPanel.SetActive(false);
        }

        if (openButton != null)
        {
            openButton.onClick.AddListener(OpenHowTo);
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseHowTo);
        }
    }

    public void OpenHowTo()
    {
        if (howToPanel != null)
        {
            howToPanel.SetActive(true);
        }
    }

    public void CloseHowTo()
    {
        if (howToPanel != null)
        {
            howToPanel.SetActive(false);
        }
    }
}
