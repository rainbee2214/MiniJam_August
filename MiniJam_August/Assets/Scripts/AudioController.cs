using UnityEngine;
using System.Collections;

public enum SoundType
{
    Create,
    Place,
    BadMove,
    Background,
    Title
}
public class AudioController : MonoBehaviour
{
    public static AudioController controller;

    void Awake()
    {
        if (controller == null)
        {
            DontDestroyOnLoad(gameObject);
            controller = this;
        }
        else if (controller != this)
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        PlaySound(SoundType.Background);
    }
    AudioSource audioSource;

    public AudioClip placeClip, createClip, badMoveClip, titleSong, backgroundSong;
    public void PlaySound(SoundType sound)
    {
        switch (sound)
        {
            case SoundType.Create:
                audioSource.PlayOneShot(createClip);
                break;
            case SoundType.Place:
                audioSource.PlayOneShot(placeClip);
                break;
            case SoundType.BadMove:
                audioSource.PlayOneShot(badMoveClip);
                break;
            case SoundType.Background:
                audioSource.clip = backgroundSong;
                audioSource.Play();
                break;
            case SoundType.Title:
                audioSource.clip = titleSong;
                audioSource.Play();
                break;
            default:
                break;
        }
    }
}
