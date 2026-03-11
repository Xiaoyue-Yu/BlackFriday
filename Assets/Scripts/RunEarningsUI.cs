using TMPro;
using UnityEngine;

public class RunEarningsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI earningsText;
    [SerializeField] private string prefix = "Total Revenue: $";

    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        if (earningsText == null || RunManager.Instance == null) return;

        earningsText.text = prefix + RunManager.Instance.RunEarnings.ToString("0.##");
    }
}

