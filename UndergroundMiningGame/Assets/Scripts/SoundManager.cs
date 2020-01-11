using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    public AudioSource backgroundPlayer;
    public AudioSource soundPlayer;
    public AudioClip[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBackground(AudioClip backgroundMusic, bool looping)
    {
        backgroundPlayer.loop = looping;
        backgroundPlayer.clip = backgroundMusic;
        backgroundPlayer.Play();
    }
    public void PlayInBackground(int index)
    {
        backgroundPlayer.loop = false;
        backgroundPlayer.clip = sounds[index];
        backgroundPlayer.Play();
    }

    public void PlaySound(int index)
    {
        soundPlayer.clip = sounds[index];
        soundPlayer.Play();
    }

    public void PlaySoundAt(int index, float position)
    {
        soundPlayer.clip = sounds[index];
        soundPlayer.PlayScheduled(position);
    }
}
