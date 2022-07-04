using UnityEngine;
using DG.Tweening;

public class Frog : Obstacle
{
    [SerializeField] Transform destinationTransform;
    [SerializeField] float duration;
    [SerializeField] float height;
    [SerializeField] Transform startTriggerTransform;
    [SerializeField] Transform endTriggerTransform;
    [SerializeField] LayerMask whatIsPlayer;

    Animator anim;

    AudioClip appearClip;

    bool reachPlayer;
    bool onAppear;

    void Awake()
    {
        Initialize();
        GetAudioClip();
    }

    #region Initial Setting
    void Initialize()
    {
        anim = GetComponent<Animator>();
    }

    void GetAudioClip()
    {
        base.hitClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Hit_01");
        appearClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Frog");
    }
    #endregion

    void Update()
    {
        base.CheckTrigger(out reachPlayer, startTriggerTransform, endTriggerTransform, whatIsPlayer);
        Jump();
    }

    void Jump()
    {
        if (!reachPlayer)
            return;

        if (!onAppear)
        {
            onAppear = true;
            transform.DOLocalJump(destinationTransform.position, height, 1, duration).SetEase(Ease.OutSine)
                     .SetDelay(0.55f)
                     .OnStart(() => 
                     {
                         SFXController.instance.PlaySFX(appearClip);
                         anim.SetTrigger("jump");
                     });
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(startTriggerTransform.position, endTriggerTransform.position);
        Gizmos.DrawWireSphere(destinationTransform.position, 0.5f);
    }
}
