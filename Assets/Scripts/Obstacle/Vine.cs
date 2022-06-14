using UnityEngine;
using DG.Tweening;

public class Vine : Obstacle
{
    [SerializeField] float destination = -0.5f;
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
        base.hitClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Hit_01");
        appearClip = Resources.Load<AudioClip>("Audio/SFX/SFX_VineAppear");
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
            SFXController.instance.PlaySFX(appearClip);

            transform.DOMoveY(destination, duration).SetEase(Ease.OutSine);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(startTriggerTransform.position, endTriggerTransform.position);
    }
}
