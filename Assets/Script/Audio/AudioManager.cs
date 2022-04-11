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

    public void PlaySFX(AudioClip clip, float delay = 0f, float pitch = 1f, float volume = 1)
    {
        AudioSource channel = FindChannel();

        if (channel != null)
        {
            channel.clip = clip;
            channel.pitch = pitch;
            channel.volume = volume;
            channel.PlayDelayed(delay);
        }
    }

    public void SetVolume(float volume)
    {
        foreach (var channel in audioSources)
        {
            
        }
    }
}
