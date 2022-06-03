using UnityEngine;

public class Thron : Obstacle
{
    void Awake()
    {
        GetAudioClip();
    }

    #region Initial Setting
    void GetAudioClip()
    {
        base.hitClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Hit_01");
    }
    #endregion
}
