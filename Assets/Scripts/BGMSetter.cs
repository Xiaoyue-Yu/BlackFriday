using UnityEngine;

public class BGMSetter : MonoBehaviour
{
    [Header("Scene BGM")]
    public AudioClip sceneBGM;

    private void Start()
    {
        if (AudioManager.Instance != null && sceneBGM != null)
        {
            AudioManager.Instance.PlayBGM(sceneBGM);
        }
    }
}