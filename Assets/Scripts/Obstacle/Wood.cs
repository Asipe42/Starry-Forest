using UnityEngine;
using DG.Tweening;

public class Wood : Obstacle
{
    [SerializeField] Transform destinationTransform;
    [SerializeField] float duration;
    [SerializeField] Transform startTriggerTransform;
    [SerializeField] Transform endTriggerTransform;
    [SerializeField] LayerMask whatIsPlayer;

    AudioClip appearClip;

    bool reachPlayer;
    bool onAppear;

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
    #endregion

    void Update()
    {
        base.CheckTrigger(out reachPlayer, startTriggerTransform, endTriggerTransform, whatIsPlayer);
        Appear();
    }

    void Appear()
    {
        if (!reachPlayer)
            return;

        if (!onAppear)
        {
            onAppear = true;
            transform.DOMoveY(destinationTransform.position.y, duration)
                     .SetEase(Ease.InCubic)
                     .OnComplete(() => SFXController.instance.PlaySFX(appearClip));
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(startTriggerTransform.position, endTriggerTransform.position);
        Gizmos.DrawWireSphere(destinationTransform.position, 0.5f);
    }
}
