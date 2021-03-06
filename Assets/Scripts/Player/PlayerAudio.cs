using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    AudioClip walkClip;  
    AudioClip jumpClip;
    AudioClip downhillClip;
    AudioClip slidingClip;
    AudioClip dashClip;
    AudioClip dashLevelUpClip;
    AudioClip takeItemClip;
    AudioClip recoverClip;
    AudioClip flyClip;
    AudioClip throwClip;

    void Awake()
    {
        GetAudioClip();
    }

    #region Initial Setting
    void GetAudioClip()
    {
        walkClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Walk");
        jumpClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Jump");
        downhillClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Downhill");
        slidingClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Sliding");
        dashClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Dash");
        dashLevelUpClip = Resources.Load<AudioClip>("Audio/SFX/SFX_DashLevelUp");
        takeItemClip = Resources.Load<AudioClip>("Audio/SFX/SFX_TakeItem");
        recoverClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Recover");
        flyClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Fly");
        throwClip = Resources.Load<AudioClip>("Audio/SFx/SFX_Throwing");

        audioSource.clip = walkClip;
    }
    #endregion

    void Update()
    {
        PlaySFX_Walk();
    }

    void PlaySFX_Walk()
    {
        if (PlayerController.instance.onGround && !PlayerController.instance.onPause)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Pause();
        }
    }

    public void PlaySFX_Jump(float delay = 0f, float pitch = 1f, float volume = 1f)
    {
        SFXController.instance.PlaySFX(jumpClip, delay, pitch, volume);
    }

    public void PlaySFX_Downhill(float delay = 0f, float pitch = 1f, float volume = 1f)
    {
        SFXController.instance.PlaySFX(downhillClip, delay, pitch, volume);
    }

    public void PlaySFX_Sliding(float delay = 0f, float pitch = 1f, float volume = 1f)
    {
        SFXController.instance.PlaySFX(slidingClip, delay, pitch, volume);
    }

    public void PlaySFX_Dash(float delay = 0f, float pitch = 1f, float volume = 1f)
    {
        SFXController.instance.PlaySFX(dashClip, delay, pitch, volume);
    }
    
    public void PlaySFX_DashLevelup(float delay = 0f, float pitch = 1f, float volume = 1f)
    {
        SFXController.instance.PlaySFX(dashLevelUpClip, delay, pitch, volume);
    }

    public void PlaySFX_TakeItem(float delay = 0f, float pitch = 1f, float volume = 1f)
    {
        SFXController.instance.PlaySFX(takeItemClip, delay, pitch, volume);
    }

    public void PlaySFX_Recover(float delay = 0f, float pitch = 1f, float volume = 1f)
    {
        SFXController.instance.PlaySFX(recoverClip, delay, pitch, volume);
    }

    public void PlaySFX_Fly(float delay = 0f, float pitch = 1f, float volume = 1f)
    {
        SFXController.instance.PlaySFX(flyClip, delay, pitch, volume);
    }

    public void PlaySFX_Throwing(float delay = 0f, float pitch = 1f, float volume = 1f)
    {
        SFXController.instance.PlaySFX(throwClip, delay, pitch, volume);
    }
}
