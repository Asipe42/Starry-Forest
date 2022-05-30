using UnityEngine;

public class SFXController : MonoBehaviour
{
    public static SFXController instance;

    [SerializeField] AudioSource[] audioSources;

    void Awake()
    {
        Initialize();
    }

    #region Initial Setting
    void Initialize()
    {
        instance = this;
    }
    #endregion

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

    AudioSource FindChannel()
    {
        foreach (var channel in audioSources)
        {
            if (!channel.isPlaying)
                return channel;
        }
        return null;
    }
}
