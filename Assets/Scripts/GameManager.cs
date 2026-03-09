using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
        }
    }

    public void StartNewGame()
    {
        Debug.Log("Start new game");
        
    }

    public void CheckLeaderBoard()
    {
        Debug.Log("Check leaderboard");
    }
    
}
