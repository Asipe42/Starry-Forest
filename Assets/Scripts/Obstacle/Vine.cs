using UnityEngine;
using DG.Tweening;

public class Vine : Obstacle
{
    AudioClip appearClip;

    [HideInInspector] public bool onAppear;

    void Awake()
    {
        GetAudioClip();
    }

    #region Initial Setting
    void GetAudioClip()
    {
        base.hitClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Hit_01");
        appearClip = Resources.Load<AudioClip>("Audio/SFX/SFX_VineAppear");
    }
    #endregion

    public void Appear()
    {
        if (onAppear)
            return;

        onAppear = true;
        SFXController.instance.PlaySFX(appearClip);

        transform.DOMoveY(-0.5f, 1f).SetEase(Ease.OutSine);
    }
}
