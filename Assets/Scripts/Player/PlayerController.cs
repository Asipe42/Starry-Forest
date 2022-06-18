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

public enum SpecialAction
{
    None = 0,
    Fly
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
    [SerializeField] float[] dashFlyColliderPosition;

    [Header("Fly")]
    [SerializeField] GameObject flyCollider;
    [SerializeField] float flyForce;

    [Header("Core")]
    public bool onTutorial;
    public bool onPause;
    public bool onFix;
    public bool reachLastFloor;
    public bool onGoal;
    public bool onGround;
    public bool onInvincibility;
    public bool canMove;
    public bool onWalk;
    public SpecialAction specialAction = SpecialAction.None;
    [SerializeField] Transform targetTransform;

    [Header("Cursor")]
    public bool[] changedCursor;
    [SerializeField] ParticleSystem cursorFirecraker;
    [SerializeField] Color[] cursorFirecrakerColors;

    [Header("Jump")]
    public bool canJump;
    public bool onJump;

    [Header("Sliding")]
    public bool canSliding;
    public bool onSliding;

    [Header("Downhill")]
    public bool canDownhill;
    public bool onDownhill;
    public bool hasDownhill;

    [Header("Dash")]
    public bool canDash;
    public bool onDash;
    public bool hasDash;

    [Header("Fly")]
    public bool canFly;
    public bool onFly;
    public bool hasFly;
    public int flyCount;

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

    AudioClip cursorClip;

    void Awake()
    {
        Initialize();
        GetAudioClip();
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
        actionList.Add("fly");

        changedCursor = new bool[2];
    }

    void GetAudioClip()
    {
        cursorClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Cursor");
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
            case "fly":
                canFly = state;
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

        if (onFly)
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

                if (!onFly)
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

        if (!onGround)
        {
            onSliding = true;
            StartCoroutine(WaitSliding());
        }
        else
        {
            SlidingLogic();
        }
    }

    IEnumerator WaitSliding()
    {
        yield return new WaitUntil(() => onGround);

        if (Input.GetKey(UseKeys.slidingKey))
        {
            SlidingLogic();
        }
        else
        {
            onSliding = false;
        }
    }

    public void SlidingLogic()
    {
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
            if ((!onFix && onTutorial) || (Input.GetKeyUp(UseKeys.slidingKey) && !onFix))
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

        if (hasDash)
        {
            onDash = true;
            hasDash = false;

            thePlayerAudio.PlaySFX_Dash(0f);

            if (IncreaseDashLevelCoroutine != null)
            {
                StopCoroutine(IncreaseDashLevelCoroutine);
            }

            IncreaseDashLevelCoroutine = IncreaseDashLevel();

            StartCoroutine(IncreaseDashLevelCoroutine);
            StartCoroutine(WaitCancleDash());
        }
    }

    public void Break()
    {
        ResetDash();
    }

    IEnumerator WaitCancleDash()
    {
        while (true)
        {
            if ((!onFix && onTutorial) || (Input.GetKeyUp(UseKeys.dashKey) && !onFix))
            {
                onDash = false;
                hasDash = true;

                if (DecreaseDashLevelCoroutine != null)
                {
                    StopCoroutine(DecreaseDashLevelCoroutine);
                }

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

                dashLevel++; dashLevel = dashLevel > DashLevel.Max ? DashLevel.Max : dashLevel;

                DashEvent.Invoke(dashLevel);
                scroll.ScrollParticle(dashLevel);
                ChangeCursor(dashLevel);
                ChangeCameraOption(dashLevel - 1, 0.75f);
                ChangeFlyColliderPosition(dashLevel - 1);

                thePlayerAudio.PlaySFX_DashLevelup(0, 1 + (float)dashLevel * 0.1f, 0.25f);
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
                yield return new WaitForSeconds(0.5f);

                dashLevel--; dashLevel = dashLevel < DashLevel.None ? DashLevel.None : dashLevel;

                DashEvent.Invoke(dashLevel);
                scroll.ScrollParticle(dashLevel);
                InitializeCursorImage(dashLevel);
                ChangeCameraOption(dashLevel, 0.5f);
                ChangeFlyColliderPosition(dashLevel);
            }

            yield return null;
        }
    }

    void ChangeCursor(DashLevel dashLevel)
    {
        if (dashLevel == DashLevel.Max && !changedCursor[1])
        {
            changedCursor[1] = true;

            Texture2D cursorImg = GameManager.instance.cursorImg[2];
            Cursor.SetCursor(cursorImg, new Vector2(cursorImg.width / 2, cursorImg.height / 2), CursorMode.ForceSoftware);

            ShowCursorFirecracker(cursorFirecrakerColors[1], 75);
        }
        else if (dashLevel > DashLevel.None && !changedCursor[0])
        {
            changedCursor[0] = true;

            Texture2D cursorImg = GameManager.instance.cursorImg[1];
            Cursor.SetCursor(cursorImg, new Vector2(cursorImg.width / 2, cursorImg.height / 2), CursorMode.ForceSoftware);

            ShowCursorFirecracker(cursorFirecrakerColors[0], 75);
        }
    }

    void ShowCursorFirecracker(Color color, int emit)
    {
        Vector3 targetPos = transform.position;
        cursorFirecraker.transform.position = targetPos;

        var temp = cursorFirecraker.main;
        temp.startColor = color;

        cursorFirecraker.Emit(emit);
    }

    void InitializeCursorImage(DashLevel dashLevel)
    {
        if (dashLevel == DashLevel.None)
        {
            changedCursor[0] = false; changedCursor[1] = false;
            Texture2D cursorImg = GameManager.instance.cursorImg[0];
            Cursor.SetCursor(cursorImg, new Vector2(cursorImg.width / 2, cursorImg.height / 2), CursorMode.ForceSoftware);
        }
    }

    void ChangeCameraOption(DashLevel dashLevel, float duration)
    {
        Camera.main.DOKill();
        Camera.main.DOOrthoSize(dashCameraSize[(int)dashLevel], duration).SetEase(Ease.OutCubic);
        Camera.main.transform.DOMoveY(dashCameraPosition[(int)dashLevel], duration).SetEase(Ease.OutCubic);
    }

    void ChangeFlyColliderPosition(DashLevel dashLevel)
    {
        if (flyCollider != null)
        {
            Vector3 flyColliderPosition = flyCollider.transform.position;
            flyColliderPosition.y = dashFlyColliderPosition[(int)dashLevel];
            flyCollider.transform.position = flyColliderPosition;
        }
    }
    #endregion

    public void PlaySpecialAction()
    {
        switch (specialAction)
        {
            case SpecialAction.None:
                break;
            case SpecialAction.Fly:
                Fly();
                break;
        }
    }

    #region Fly
    void Fly()
    {
        if (!canMove)
            return;

        if (!canFly)
            return;

        if (onGround)
            return;

        if (onFly)
            return;

        if (flyCount > 0)
        {
            onFly = true;
            hasFly = true;

            flyCount--;
            UIManager.instance.dandelionStack.CheckDandelionCount();

            thePlayerMovement.Movement_Fly(true);
            thePlayerAnimation.PlayFlyAnimation(true);
            thePlayerAudio.PlaySFX_Fly();
            thePlayerParticle.PlayDandelion(true);

            StartCoroutine(WaitFly());
        }
    }

    public void CancleFly()
    {
        hasFly = false;
    }

    IEnumerator WaitFly()
    {
        while (onFly)
        {
            if (Input.GetKeyDown(UseKeys.specialKey))
            {
                rigid.velocity = Vector2.zero;
            }

            if (Input.GetKey(UseKeys.specialKey))
            {
                rigid.AddForce(Vector2.up * flyForce);
            }

            if (!hasFly || onGround)
            {
                onFly = false;
                hasFly = false;

                thePlayerMovement.Movement_Fly(false);
                thePlayerAnimation.PlayFlyAnimation(false);
                thePlayerParticle.PlayDandelion(false);
            }
            yield return null;
        }
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

        ResetDash();

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

    void ResetDash()
    {
        dashLevel = DashLevel.One;

        DashEvent.Invoke(dashLevel);
        scroll.ScrollParticle(dashLevel);
        InitializeCursorImage(dashLevel);
        ChangeCameraOption(dashLevel, 0.5f);
        ChangeFlyColliderPosition(dashLevel);
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

        canMove = false;
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
        StartCoroutine(UIManager.instance.hud.HideEveryElements(0f, 0.5f));

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
        StartCoroutine(UIManager.instance.hud.HideEveryElements(0f, 1f));

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
                thePlayerAnimation.SetEveryAnimationParameter(false);

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
