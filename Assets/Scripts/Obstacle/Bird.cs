using UnityEngine;
using DG.Tweening;

public class Bird : Obstacle
{
    [SerializeField] Vector2 destination = new Vector2(-0.5f, -1.25f);
    [SerializeField] float duration;

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
        appearClip = Resources.Load<AudioClip>("Audio/SFX/SFX_BirdAppear");
    }
    #endregion

    public void Appear()
    {
        if (onAppear)
            return;

        onAppear = true;
        SFXController.instance.PlaySFX(appearClip);
        transform.DOMove(destination, duration).SetEase(Ease.OutSine);
    }
}
