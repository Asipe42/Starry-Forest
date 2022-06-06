using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum DashLevel
{
    None = 0,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Max,
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Colliders")]
    [SerializeField] BoxCollider2D theCollider;

    [Header("Dash")]
    public DashLevel dashLevel = DashLevel.None;
    [SerializeField] float[] dashTime;
    [SerializeField] float[] dashCameraSize;
    [SerializeField] float[] dashCameraPosition;

    public bool onTutorial;
    public bool onPause;
    public bool onFix;
    public bool reachLastFloor;
    public bool onGoal;
    public bool canMove;
    public bool canJump;
    public bool canSliding;
    public bool canDownhill;
    public bool canDash;
    public bool onGround;
    public bool onWalk;
    public bool onJump;
    public bool onDownhill;
    public bool onSliding;
    public bool onDash;
    public bool hasDownhill;
    public bool hasDash;
    public bool onInvincibility;

    [SerializeField] Transform targetTransform;

    public PlayerAnimation thePlayerAnimation { get; private set; }
    public PlayerAudio thePlayerAudio { get; private set; }
    public PlayerMovement thePlayerMovement { get; private set; }
    public PlayerParticle thePlayerParticle { get; private set; }
    public Status theStatus { get; private set; }
    public Scroll scroll { get; private set; }

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;

    IEnumerator IncreaseDashLevelCoroutine;
    IEnumerator DecreaseDashLevelCoroutine;

    List<string> actionList = new List<string>();

    public static event Action<DashLevel> DashEvent;
    public static event Action<bool> deadEvent;

    void Awake()
    {
        Initialize();
    }

    #region Initial Setting
    void Initialize()
    {
        instance = this;

        thePlayerAnimation = GetComponent<PlayerAnimation>();
        thePlayerAudio = GetComponent<PlayerAudio>();
        thePlayerMovement = GetComponent<PlayerMovement>();
        thePlayerParticle = GetComponent<PlayerParticle>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        scroll = GameObject.FindObjectOfType<Scroll>();

        theStatus = new Status();

        actionList.Add("jump");
        actionList.Add("sliding");
        actionList.Add("downhill");
        actionList.Add("dash");
    }
    #endregion

    /// <summary>
    /// name 행동의 제어 상태를 state로 바꾼다
    /// </summary>
    /// <param name="name"></param>
    /// <param name="state"></param>
    public void PermitAction(string name, bool state = true)
    {
        switch (name)
        {
            case "jump":
                canJump = state;
                break;
            case "sliding":
                canSliding = state;
                break;
            case "downhill":
                canDownhill = state;
                break;
            case "dash":
                canDash = state;
                break;
        }
    }

    /// <summary>
    /// 모든 행동 상태를 제어할 수 있도록 허가한다.
    /// </summary>
    /// <param name="state"></param>
    public void PermitEveryAction(bool state)
    {
        foreach (var action in actionList)
        {
            PermitAction(action, state);
        }
    }

    #region Jump
    public void Jump()
    {
        if (!canMove)
            return;

        if (!canJump)
            return;

        if (onSliding)
            return;

        onWalk = false;
        onJump = true;

        thePlayerMovement.Movement_Jump();
        thePlayerAnimation.PlayJumpAnimation(true);
        thePlayerAudio.PlaySFX_Jump(0);
    }
    #endregion

    #region Downhill
    public void Downhill()
    {
        if (!canMove)
            return;

        if (!canDownhill)
            return;

        if (onDownhill)
            return;

        if (onSliding)
            return;

        if (hasDownhill)
        {
            onDownhill = true;
            hasDownhill = false;

            thePlayerAnimation.PlayJumpAnimation(false);
            thePlayerMovement.Movement_Downhill(true);
            thePlayerAnimation.PlayDownhillAnimation(onDownhill);
            thePlayerAudio.PlaySFX_Downhill(0);

            StartCoroutine(CancleDownhill());
        }
    }

    IEnumerator CancleDownhill()
    {
        while (onDownhill)
        {
            if ((!onFix && onTutorial) ||(Input.GetKeyUp(UseKeys.jumpKey) && !onFix))
            {
                onDownhill = false;

                thePlayerMovement.Movement_Downhill(onDownhill);
            }

            yield return null;
        }

        yield return null;
    }
    #endregion

    #region Sliding
    public void Sliding()
    {
        if (!canMove)
            return;

        if (!canSliding)
            return;

        if (onSliding)
            return;

        if (onJump || onDownhill)
            return;

        onWalk = false;
        onSliding = true;

        thePlayerAnimation.PlaySlidingAnimation(onSliding);
        thePlayerAudio.PlaySFX_Sliding(0);
        thePlayerParticle.PlaySlidingDust(onSliding);

        theCollider.size = new Vector2(theCollider.size.x, 0.75f);
        theCollider.offset = new Vector2(theCollider.offset.x, -0.8f);

        StartCoroutine(CancleSliding());
    }

    IEnumerator CancleSliding()
    {
        while (onSliding)
        {
            if ((!onFix && onTutorial) || (Input.GetKeyUp(UseKeys.SlidingKey) && !onFix))
            {
                onWalk = true;
                onSliding = false;

                theCollider.size = new Vector2(theCollider.size.x, 1.9f);
                theCollider.offset = new Vector2(theCollider.offset.x, -0.2f);

                thePlayerAnimation.PlaySlidingAnimation(onSliding);
                thePlayerParticle.PlaySlidingDust(onSliding);
            }

            yield return null;
        }

        yield return null;
    }
    #endregion

    #region Dash
    public void Dash()
    {
        if (!canMove)
            return;

        if (!canDash)
            return;

        if (!onGround)
            return;

        if (hasDash)
        {
            onDash = true;
            hasDash = false;

            thePlayerAudio.PlaySFX_Dash(0f);

            if (IncreaseDashLevelCoroutine != null)
                StopCoroutine(IncreaseDashLevelCoroutine);

            IncreaseDashLevelCoroutine = IncreaseDashLevel();
            StartCoroutine(IncreaseDashLevelCoroutine);

            StartCoroutine(CancleDash());
        }
    }

    IEnumerator CancleDash()
    {
        while (true)
        {
            if ((!onFix && onTutorial) || (Input.GetKeyUp(UseKeys.dashKey) && !onFix))
            {
                onDash = false;

                StartCoroutine(ChargeDash());

                if (DecreaseDashLevelCoroutine != null)
                    StopCoroutine(DecreaseDashLevelCoroutine);

                DecreaseDashLevelCoroutine = DecraseDashLevel();
                StartCoroutine(DecreaseDashLevelCoroutine);
            }

            yield return null;
        }
    }

    IEnumerator IncreaseDashLevel()
    {
        while (onDash)
        {
            if (dashLevel < DashLevel.Max)
            {
                yield return new WaitForSeconds(dashTime[(int)dashLevel]);

                dashLevel++;
                dashLevel = dashLevel > DashLevel.Max ? DashLevel.Max : dashLevel;
                DashEvent.Invoke(dashLevel);
                scroll.ScrollParticle(dashLevel);
                thePlayerAudio.PlaySFX_DashLevelup(0, 1 + (float)dashLevel * 0.1f, 0.25f);
                Camera.main.DOKill();
                Camera.main.DOOrthoSize(dashCameraSize[(int)dashLevel - 1], 0.75f).SetEase(Ease.OutCubic);
                Camera.main.transform.DOMoveY(dashCameraPosition[(int)dashLevel - 1], 0.75f).SetEase(Ease.OutCubic);
            }

            yield return null;
        }
    }

    IEnumerator DecraseDashLevel()
    {
        while (!onDash)
        {
            if (dashLevel > DashLevel.None)
            {
                yield return new WaitForSeconds(0.15f);

                dashLevel--;
                dashLevel = dashLevel < DashLevel.None ? DashLevel.None : dashLevel;
                DashEvent.Invoke(dashLevel);
                scroll.ScrollParticle(dashLevel);
                Camera.main.DOKill();
                Camera.main.DOOrthoSize(dashCameraSize[(int)dashLevel], 0.5f).SetEase(Ease.OutCubic);
                Camera.main.transform.DOMoveY(dashCameraPosition[(int)dashLevel], 0.75f).SetEase(Ease.OutCubic);
            }

            yield return null;
        }
    }

    IEnumerator ChargeDash()
    {
        while (dashLevel > DashLevel.None)
        {
            yield return null;
        }

        hasDash = true;
    }
    #endregion

    #region Knockback
    /// <summary>
    /// force만큼 direction으로 힘을 가한다.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="force"></param>
    public void Knockback(Vector2 direction, float force)
    {
        rigid.AddForce(new Vector2(-1f, 0.75f) * 2f, ForceMode2D.Impulse);
    }
    #endregion

    #region Damaged
    IEnumerator Invincibility(float duration, float alphaValue)
    {
        UIManager.instance.bloodScreen.BloodScreenEffect(0.25f, 0.75f);
        onInvincibility = true;
        spriteRenderer.color = new Color(1, 1, 1, alphaValue);

        yield return new WaitForSeconds(duration);

        onInvincibility = false;
        spriteRenderer.color = new Color(1, 1, 1, 1f);
    }

    public void OnDamaged(int damage, AudioClip clip)
    {
        if (onInvincibility)
            return;

        theStatus.currentHp -= damage;

        if (theStatus.currentHp <= 0)
        {
            StartCoroutine(Dead());
        }

        StartCoroutine(Invincibility(0.8f, 0.5f));

        SFXController.instance.PlaySFX(clip);
        UIManager.instance.heart.CheckHp(theStatus.currentHp);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(Camera.main.DOShakePosition(
                                        duration: 0.75f,
                                        new Vector3(0.3f, 0f, 0f),
                                        vibrato: 5,
                                        randomness: 90))
                                     .SetEase(Ease.OutQuad);
    }

    void CameraSize(float x)
    {
        Camera.main.orthographicSize = x;
    }
    #endregion

    #region Dead
    public IEnumerator Dead()
    {
        float duration = 8f;

        deadEvent.Invoke(false);
        thePlayerAnimation.PlayDeadAnimation();
        StopAction();
        StartCoroutine(DeadDirection());
        yield return new WaitForSeconds(duration);
        DeadLogic();
    }
    #endregion

    IEnumerator DeadDirection()
    {
        Vector2 knockbackDirection = new Vector2(-1f, 0.75f);
        float knockbackForce = 2f;
        float duration = 2f;

        Knockback(knockbackDirection, knockbackForce);

        UIManager.instance.darkenScreen.DarkenScreenEffect(1, duration);
        UIManager.instance.hud.HideHeartBox(0.5f);
        UIManager.instance.hud.HidePDBox(0.5f);
        UIManager.instance.hud.HideRSBox(0.5f);
        yield return new WaitForSeconds(duration + 0.5f);

        UIManager.instance.resultSign.ShowResultSign("완주 실패");
        yield return new WaitUntil(() => UIManager.instance.resultSign.endDirecting);
        UIManager.instance.fadeScreen.FadeScreenEffect(1, 1f);
    }

    void DeadLogic()
    {
        DecreaseLife();
        Loading.LoadScene("Map");
    }

    void DecreaseLife()
    {
        GameManager.life--;
    }

    #region Take Item
    public void TakeItem(int score)
    {
        UIManager.instance.score?.CheckScore(score);
        thePlayerAudio.PlaySFX_TakeItem();
        thePlayerParticle.PlayTakeItem();
    }

    public void Recover(int value)
    {
        thePlayerAudio.PlaySFX_Recover();
        thePlayerParticle.PlayRecover();

        if (theStatus.maxHp <= theStatus.currentHp)
            return;

        theStatus.currentHp += value;

        UIManager.instance.heart.CheckHp(theStatus.currentHp);
    }
    #endregion

    public IEnumerator GoalAction(LastFloorState lastFloorState)
    {
        yield return new WaitUntil(() => onGround);

        canMove = false;

        if (IncreaseDashLevelCoroutine != null)
            StopCoroutine(IncreaseDashLevelCoroutine);

        UIManager.instance.darkenScreen.DarkenScreenEffect(0.4f, 1f);
        UIManager.instance.hud.HideHeartBox(1f);
        UIManager.instance.hud.HidePDBox(1f);
        UIManager.instance.hud.HideRSBox(1f);

        switch (lastFloorState)
        {
            case LastFloorState.Tutorial:
            case LastFloorState.Normal:
                onWalk = true;
                transform.DOMoveX(targetTransform.position.x, 3f)
                         .OnComplete(() => 
                         { 
                             onGoal = true;
                             dashLevel = DashLevel.None;
                         });
                break;
            case LastFloorState.Bornfire:
                onWalk = false;
                dashLevel = DashLevel.None;
                thePlayerAnimation.PlayDownhillAnimation(false);
                thePlayerAnimation.PlayJumpAnimation(false);
                thePlayerAnimation.PlaySlidingAnimation(false);
                thePlayerAnimation.PlayDashAnimation(false);
                thePlayerAnimation.PlayWalkAnimation(false);
                yield return new WaitForSeconds(1.5f);
                onGoal = true;
                break;
        }
    }

    /// <summary>
    /// 입력과 움직임을 차단한다.
    /// </summary>
    public void StopAction()
    {
        InputManager.instance.onLock = true;
        canMove = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if (!reachLastFloor && !onSliding)
                onWalk = true;

            if (rigid.velocity.y <= 0)
                onJump = false;

            onDownhill = false;
            hasDownhill = true;
            rigid.gravityScale = 2f;

            if (canMove)
            {
                if (rigid.velocity.y <= 0)
                    thePlayerAnimation.PlayJumpAnimation(false);

                thePlayerAnimation.PlayDownhillAnimation(false);
                thePlayerAnimation.PlayWalkAnimation(true);
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            onGround = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            onGround = false;
        }
    }
}
