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

    public void PlaySFX(AudioClip clip, float delay = 0f, float pitch = 1f)
    {
        AudioSource channel = FindChannel();

        if (channel != null)
        {
            channel.clip = clip;
            channel.pitch = pitch;
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
