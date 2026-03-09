using System;
using UnityEngine;
using TMPro;

public class ShopTimer : MonoBehaviour
{
    public static ShopTimer Instance;

    [Header("UI")] 
    [SerializeField] private TextMeshProUGUI clockText;

    [Header("Settings")]
    [SerializeField] private float realWorldDuration = 10f;
    [SerializeField] private int simulatedStartHour = 8;
    [SerializeField] private int simulatedCloseHour = 18;

    public static event Action OnStoreOpen;
    public static event Action OnStoreClose;

    private float elapsedRealSec = 0f;
    public bool isStoreOpen { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            UpdateClockUI();
        }
        else
        {
            // If a new Timer exists in the next scene, give it our clock reference then kill it
            Instance.AssignNewUI(this.clockText);
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (RunManager.Instance != null)
            RunManager.Instance.OnRunStart += InstanceOnOnRunStart;
    }

    // Call this to reconnect the UI when a new scene loads
    public void AssignNewUI(TextMeshProUGUI newClockText)
    {
        clockText = newClockText;
        UpdateClockUI();
    }

    private void Update()
    {
        if (!isStoreOpen) return;

        elapsedRealSec += Time.deltaTime;
        UpdateClockUI();

        // Use realWorldDuration directly
        if (elapsedRealSec >= realWorldDuration)
        {
            CloseStore();
        }
    }

    private void InstanceOnOnRunStart()
    {
        OpenStore();
    }

    private void OpenStore()
    {
        isStoreOpen = true;
        elapsedRealSec = 0f;
        OnStoreOpen?.Invoke(); 
    }

    private void CloseStore()
    {
        isStoreOpen = false;
        if (clockText != null) clockText.text = "CLOSED";
        OnStoreClose?.Invoke();
    }
    
    private void UpdateClockUI()
    {
        if (clockText == null) return;
        
        if (!isStoreOpen) 
        {
            clockText.text = "CLOSED"; 
            return;
        }

        float progress = Mathf.Clamp01(elapsedRealSec / realWorldDuration);
        
        float simulatedTotalMinutes = (simulatedCloseHour - simulatedStartHour) * 60f;
        float currentSimulatedMinutes = (simulatedStartHour * 60f) + (progress * simulatedTotalMinutes);
        
        int hours = Mathf.FloorToInt(currentSimulatedMinutes / 60f);
        int minutes = Mathf.FloorToInt(currentSimulatedMinutes % 60f);
        
        string amPm = hours >= 12 ? "PM" : "AM";
        int displayHour = hours % 12;
        if (displayHour == 0) displayHour = 12;

        clockText.text = $"{displayHour:00}:{minutes:00} {amPm}";
    }
}