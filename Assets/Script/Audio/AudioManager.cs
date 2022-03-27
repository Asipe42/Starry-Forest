using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource[] audioSources;

    void Awake()
    {
        instance = this;
    }

    public void PlaySFX(AudioClip clip, float delay = 0)
    {
        AudioSource channel = FindChannel();

        if (channel != null)
        {
            channel.clip = clip;
            channel.PlayDelayed(delay);
        }
    }

    AudioSource FindChannel()
    {
        foreach (var channel in audioSources)
        {
            if (!channel.isPlaying)
                return channel;
        }

        Debug.LogWarning("all channel is streaming");
        return null;
    }
}
