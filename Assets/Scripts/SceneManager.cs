using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    public string storeSceneName;
    public string customerSceneName;
    public string titleSceneName;
    public string checkoutSceneName;
    public string resultSceneName;
    
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

    public void LoadStoreScene()
    {
        if (storeSceneName == "") return;
        SceneManager.LoadScene(storeSceneName);
    }

    public void LoadCheckoutScene()
    {
        if (checkoutSceneName == "") return;
        SceneManager.LoadScene(checkoutSceneName);
    }

    public void LoadCustomerScene()
    {
        if (customerSceneName == "") return;
        SceneManager.LoadScene(customerSceneName);
    }

    public void LoadTitleScene()
    {
        if (titleSceneName == "") return;
        SceneManager.LoadScene(titleSceneName);
    }

    public void LoadResultScene()
    {
        if (resultSceneName == "") return;
        SceneManager.LoadScene(resultSceneName);
    }
}
