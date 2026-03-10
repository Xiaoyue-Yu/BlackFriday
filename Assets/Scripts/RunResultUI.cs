using UnityEngine;
using TMPro; 
using UnityEngine.UI;

public class RunResultUI : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] private TextMeshProUGUI earningsText;
    [SerializeField] private TextMeshProUGUI customerText;
    [SerializeField] private TextMeshProUGUI clothText;
    [SerializeField] private TextMeshProUGUI failedText;

    [Header("Buttons")]
    [SerializeField] private Button restartButton;

    private void Start()
    {
        // 1. Pull data from the Singleton
        if (RunManager.Instance != null)
        {
            earningsText.text = $"${RunManager.Instance.RunEarnings}";
            customerText.text = $"Customer: {RunManager.Instance.CustomerFulfilled}";
            clothText.text = $"Cloth: {RunManager.Instance.ClothSold}";
            failedText.text = $"Failed: {RunManager.Instance.CustomerFailed}";
        }
        else
        {
            Debug.LogWarning("RunManager Instance is null! Start from the main menu to test.");
        }
        
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartClicked);
        }
    }

    private void OnRestartClicked()
    {
        if (RunManager.Instance != null)
        {
            RunManager.Instance.StartNewRun();
        }
    }
}