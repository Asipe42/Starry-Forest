using UnityEngine;
using DG.Tweening;

public class Wood : Obstacle
{
    [SerializeField] Transform destinationTransform;
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
        base.hitClip = Resources.Load<AudioClip>("Audio/SFX/SFX_WoodHit");
        appearClip = Resources.Load<AudioClip>("Audio/SFX/SFX_WoodAppear");
    }

    public void Appear()
    {
        if (onAppear)
            return;

        onAppear = true;
        transform.DOMove(destinationTransform.position, duration)
                 .SetEase(Ease.OutQuad)
                 .OnComplete(() => SFXController.instance.PlaySFX(appearClip));
    }
    #endregion
}
