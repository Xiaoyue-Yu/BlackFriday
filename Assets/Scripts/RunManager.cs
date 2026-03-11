using System;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    public CustomerGO curCustomerGO = null;

    public event Action OnRunStart;
    public event Action<float> OnRunEnd;

    [SerializeField] private float runEarnings = 0f;
    [SerializeField] private int customerFulfilled = 0;
    [SerializeField] private int customerFailed = 0;
    [SerializeField] private int runScore = 0;
    [SerializeField] private int clothSold = 0;

    private bool hasRunEnded;

    public int ClothSold
    {
        get => clothSold;
        set => clothSold = value;
    }

    public int CustomerFailed
    {
        get => customerFailed;
        set => customerFailed = value;
    }

    public int RunScore
    {
        get => runScore;
        set => runScore = value;
    }

    public int CustomerFulfilled
    {
        get => customerFulfilled;
        set => customerFulfilled = value;
    }

    public float RunEarnings
    {
        get => runEarnings;
        set => runEarnings = value;
    }

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
        CheckoutManager.Instance.OnCheckOutFinished += InstanceOnOnCheckOutFinished;
    }

    private void InstanceOnOnCheckOutFinished(bool isBuying)
    {
        curCustomerGO = null;
        runScore = 0;
    }

    private void OnDisable()
    {
        if (CheckoutManager.Instance != null)
        {
            CheckoutManager.Instance.OnCheckOutFinished -= InstanceOnOnCheckOutFinished;
        }
    }

    public void StartNewRun()
    {
        SceneLoader.Instance.LoadStoreScene();
        Debug.Log("Start new Run");

        runEarnings = 0f;
        customerFulfilled = 0;
        customerFailed = 0;
        clothSold = 0;
        runScore = 0;
        hasRunEnded = false;

        OnRunStart?.Invoke();
    }

    public void EndRun()
    {
        if (hasRunEnded) return;
        hasRunEnded = true;

        Debug.Log("Ending Run!");
        OnRunEnd?.Invoke(runEarnings);
        SceneLoader.Instance.LoadResultScene();
    }
}
