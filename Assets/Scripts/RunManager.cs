using System;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    public CustomerGO curCustomerGO = null;

    public event Action OnRunStart;
    public event Action<float> OnRunEnd;     // earnings
    

    [SerializeField]private float runEarnings = 0f;
    [SerializeField]private int customerFulfilled = 0;
    [SerializeField] private int customerFailed = 0;
    [SerializeField]private int runScore = 0;
    [SerializeField] private int clothSold = 0;

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
        ShopTimer.OnStoreClose += EndRun;
        CheckoutManager.Instance.OnCheckOutFinished += InstanceOnOnCheckOutFinished;
    }

    private void InstanceOnOnCheckOutFinished(bool isBuying)
    {
        // clear customer
        curCustomerGO = null;
        runScore = 0;
    }

    private void OnDisable()
    {
        ShopTimer.OnStoreClose -= EndRun;
        CheckoutManager.Instance.OnCheckOutFinished -= InstanceOnOnCheckOutFinished;
    }

    public void StartNewRun()
    {
        
        SceneLoader.Instance.LoadStoreScene();
        Debug.Log("Start new Run");
        // Reset all stats for the new run
        runEarnings = 0f;
        customerFulfilled = 0;
        customerFailed = 0;
        clothSold = 0;
        runScore = 0;
        OnRunStart?.Invoke();
    }

    public void EndRun()
    {
        Debug.Log("Ending Run!");
        OnRunEnd?.Invoke(runEarnings);
        // TODO: save score
        // transition to title
        SceneLoader.Instance.LoadResultScene();
    }
    
}
