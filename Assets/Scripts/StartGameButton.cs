using System;
using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    public Button startgameButton;

    private void Start()
    {
        if (startgameButton)
        {
            startgameButton.onClick.AddListener(GameManager.Instance.StartNewGame);
        }
    }
}
