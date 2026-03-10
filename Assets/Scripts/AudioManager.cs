using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Player")]
    public AudioSource bgmSource;

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

    public void PlayBGM(AudioClip newClip)
    {
        if (bgmSource.clip == newClip) return;

        bgmSource.clip = newClip;
        bgmSource.Play();
    }
}