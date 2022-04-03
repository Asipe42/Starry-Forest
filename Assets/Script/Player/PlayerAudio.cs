using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip downhillClip;
    [SerializeField] AudioClip slidingClip;
    [SerializeField] AudioClip dashClip;
    [SerializeField] AudioClip dashLevelUpClip;
    [SerializeField] AudioClip takeItemClip;

    void Awake()
    {
        jumpClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Jump");
        downhillClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Downhill");
        slidingClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Sliding");
        dashClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Dash");
        dashLevelUpClip = Resources.Load<AudioClip>("Audio/SFX/SFX_DashLevelUp");
        takeItemClip = Resources.Load<AudioClip>("Audio/SFX/SFX_TakeItem");
    }

    public void PlaySFX_Jump(float delay = 0f)
    {
        AudioManager.instance.PlaySFX(jumpClip, delay);
    }

    public void PlaySFX_Downhill(float delay = 0f)
    {
        AudioManager.instance.PlaySFX(downhillClip, delay);
    }

    public void PlaySFX_Sliding(float delay = 0f)
    {
        AudioManager.instance.PlaySFX(slidingClip, delay);
    }

    public void PlaySFX_Dash(float delay = 0f)
    {
        AudioManager.instance.PlaySFX(dashClip, delay);
    }
    
    public void PlaySFX_DashLevelup(float delay = 0f, float pitch = 1f)
    {
        AudioManager.instance.PlaySFX(dashLevelUpClip, delay, pitch);
    }

    public void PlaySFX_TakeItem(float delay = 0f)
    {
        AudioManager.instance.PlaySFX(takeItemClip, delay);
    }
}
