using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayClick()
    {
        audioSource.Play();
    }
}