using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Texture2D cursorTexture;
    

    // public event Action OnGameStart;
    
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
            return;
        }
        //Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        
    }

    private void Start()
    {
        
    }

    public void StartNewGame()
    {
        Debug.Log("Start new game");
        RunManager.Instance.StartNewRun();
        
    }

    public void CheckLeaderBoard()
    {
        Debug.Log("Check leaderboard");
    }
    
}
