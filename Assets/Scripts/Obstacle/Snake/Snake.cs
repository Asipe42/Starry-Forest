using System.Collections;
using UnityEngine;
using Spine.Unity;

public class Snake : Obstacle
{
    [SerializeField] Transform startTriggerTransform;
    [SerializeField] Transform endTriggerTransform;
    [SerializeField] LayerMask whatIsPlayer;
    [SerializeField] AnimationReferenceAsset[] animClips;
    [SerializeField] float idleWaitTime;

    SkeletonAnimation skeletonAnimation;
    Spine.AnimationState snakeAnimationState;

    float timer;

    bool reachPlayer;
    bool onHit;

    IEnumerator WaitIdleCoroutine;

    public enum SnakeState
    {
        Idle,
        Appear,
        Disappear,
        Attack,
        Eat
    }

    SnakeState myState;

    void Awake()
    {
        Initialize();
        GetAudioClip();
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        base.CheckTrigger(out reachPlayer, startTriggerTransform, endTriggerTransform, whatIsPlayer);
        CheckHit();
    }

    #region Initial Setting
    void Initialize()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        snakeAnimationState = skeletonAnimation.AnimationState;
    }

    void GetAudioClip()
    {
        base.hitClip = Resources.Load<AudioClip>("Audio/SFX/SFX_SnakeHit");
    }
    #endregion

    public void ChangeAnimState(SnakeState state)
    {
        myState = state;
        SyncAnimation();
    }

    #region SnakeFSM
    void SyncAnimation()
    {
        switch (myState)
        {
            case SnakeState.Idle:
                SetIdleAnimation();
                PlayAnimation(animClips[0], true);
                break;
            case SnakeState.Appear:
                SetAppearAnimation();
                PlayAnimation(animClips[1], false);
                break;
            case SnakeState.Disappear:
                SetDisappearAnimation();
                PlayAnimation(animClips[2], false);
                break;
            case SnakeState.Attack:
                SetAttackAnimation();
                PlayAnimation(animClips[3], false);
                break;
            case SnakeState.Eat:
                SetEatAnimation();
                PlayAnimation(animClips[4], false);
                break;
        }
    }

    void PlayAnimation(AnimationReferenceAsset animClip, bool loop)
    {
        snakeAnimationState.SetAnimation(0, animClip, loop);
    }

    void SetAppearAnimation()
    {
        snakeAnimationState.Dispose += AppearToIdle;
    }

    void SetIdleAnimation()
    {
        snakeAnimationState.Dispose -= AppearToIdle;

        snakeAnimationState.Start += WaitIdle;
    }

    void SetAttackAnimation()
    {
        snakeAnimationState.Start -= WaitIdle;

        snakeAnimationState.End += DisableSnake;
    }

    void SetEatAnimation()
    {
        snakeAnimationState.Start -= WaitIdle;

        snakeAnimationState.End += WaitDisappear;
    }

    void SetDisappearAnimation()
    {
        snakeAnimationState.End -= WaitDisappear;
    }

    void AppearToIdle(Spine.TrackEntry trackEntry)
    {
        ChangeAnimState(SnakeState.Idle);
    }

    void WaitIdle(Spine.TrackEntry trackEntry)
    {
        timer = Time.time;

        WaitIdleCoroutine = WaitIdleLogic();
        StartCoroutine(WaitIdleCoroutine);
    }

    void DisableSnake(Spine.TrackEntry trackEntry)
    {
        float cooltime = 3f;

        StartCoroutine(DisableSnakeLogic(cooltime));
    }

    void WaitDisappear(Spine.TrackEntry trackEntry)
    {
        float cooltime = 3f;

        StartCoroutine(WaitDisappearLogic(cooltime));
    }

    IEnumerator WaitIdleLogic()
    {
        bool onThrowing = false;

        while (Time.time >= timer + idleWaitTime)
        {
            if (true)
            {
                onThrowing = true;
                ChangeAnimState(SnakeState.Attack);

                if (WaitIdleCoroutine != null)
                    StopCoroutine(WaitIdleCoroutine);
            }

            yield return null;
        }

        if (!onThrowing)
            ChangeAnimState(SnakeState.Attack);
    }

    IEnumerator DisableSnakeLogic(float cooltime)
    {
        yield return new WaitForSeconds(cooltime);

        gameObject.SetActive(false);
    }


    IEnumerator WaitDisappearLogic(float cooltime)
    {
        yield return new WaitForSeconds(cooltime);

        ChangeAnimState(SnakeState.Disappear);
    }
    #endregion

    #region Hit
    void MoveTriggerTransform()
    {

    }

    void CheckHit()
    {
        if (reachPlayer)
        {
            if (!onHit)
            {
                onHit = true;
                StartCoroutine(WaitHitCooltime(4));
                PlayerController.instance.OnDamaged(damage, hitClip);
            }
        }
    }

    IEnumerator WaitHitCooltime(float cooltime)
    {
        yield return new WaitForSeconds(cooltime);
        onHit = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(startTriggerTransform.position, endTriggerTransform.position);
    }
    #endregion
}
