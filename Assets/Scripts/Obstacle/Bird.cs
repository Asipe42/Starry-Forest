using UnityEngine;
using DG.Tweening;

public class Bird : Obstacle
{
    [SerializeField] Vector2 destination = new Vector2(-0.5f, -1.25f);
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

    void Update()
    {
        base.CheckTrigger(out reachPlayer, startTriggerTransform, endTriggerTransform, whatIsPlayer);
        Appear();
    }

    #region Initial Setting
    void GetAudioClip()
    {
        base.hitClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Hit_01");
        appearClip = Resources.Load<AudioClip>("Audio/SFX/SFX_BirdAppear");
    }
    #endregion

    void Appear()
    {
        if (!reachPlayer)
            return;

        if (!onAppear)
        {
            onAppear = true;
            SFXController.instance.PlaySFX(appearClip);
            transform.DOMove(destination, duration).SetEase(Ease.OutSine);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(startTriggerTransform.position, endTriggerTransform.position);
    }
}
