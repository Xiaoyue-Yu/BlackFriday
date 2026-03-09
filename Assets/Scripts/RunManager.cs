using System;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    public CustomerGO curCustomerGO = null;

    public event Action OnRunStart;
    public event Action<float> OnRunEnd;     // earnings

    [SerializeField] private float runEarnings = 0f;
    [SerializeField] private int customerFulfilled = 0;
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        ShopTimer.OnStoreClose += EndRun;
    }

    private void OnDisable()
    {
        ShopTimer.OnStoreClose -= EndRun;
    }

    public void StartNewRun()
    {
        Debug.Log("Start new Run");
        runEarnings = 0f;
        customerFulfilled = 0;
        OnRunStart?.Invoke();
    }

    public void EndRun()
    {
        Debug.Log("Ending Run!");
        OnRunEnd?.Invoke(runEarnings);
    }
}
