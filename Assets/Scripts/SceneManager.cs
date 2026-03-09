using System;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    public string storeSceneName;
    public string customerSceneName;
    public string titleSceneName;
    public string checkoutSceneName;
    
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
        CustomerGO.OnCustomerServeStart += CustomerGOOnOnCustomerServeStart;
    }

    private void CustomerGOOnOnCustomerServeStart(string obj)
    {
        if (storeSceneName == "") return;
        UnityEngine.SceneManagement.SceneManager.LoadScene(storeSceneName);
    }
}
