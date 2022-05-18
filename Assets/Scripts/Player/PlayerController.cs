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

    public bool onTutorial = true;
    public bool onPause;
    public bool onFix;
    public bool reachLastFloor;
    public bool canMove = true;
    public bool canJump;
    public bool canSliding;
    public bool canDownhill;
    public bool canDash;
    public bool onGround;
    public bool onWalk = true;
    public bool onJump;
    public bool onDownhill;
    public bool onSliding;
    public bool onDash;
    public bool hasDownhill = true;
    public bool hasDash = true;
    public bool onInvincibility;

    PlayerAnimation thePlayerAnimation;
    PlayerAudio thePlayerAudio;
    PlayerMovement thePlayerMovement;
    PlayerParticle thePlayerParticle;
    Status theStatus;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;

    IEnumerator IncreaseDashLevelCoroutine;
    IEnumerator DecreaseDashLevelCoroutine;

    List<string> actionList = new List<string>();

    public static event Action<DashLevel> DashAction;

    void Awake()
    {
        instance = this;

        thePlayerAnimation = GetComponent<PlayerAnimation>();
        thePlayerAudio = GetComponent<PlayerAudio>();
        thePlayerMovement = GetComponent<PlayerMovement>();
        thePlayerParticle = GetComponent<PlayerParticle>();
        theStatus = GetComponent<Status>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

        actionList.Add("jump");
        actionList.Add("sliding");
        actionList.Add("downhill");
        actionList.Add("dash");
    }

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
                DashAction.Invoke(dashLevel);
                thePlayerAudio.PlaySFX_DashLevelup(0, 1 + (float)dashLevel * 0.1f, 0.25f);
                Camera.main.DOKill();
                Camera.main.DOOrthoSize(dashCameraSize[(int)dashLevel - 1], 0.75f).SetEase(Ease.OutCubic);
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
                DashAction.Invoke(dashLevel);
                Camera.main.DOKill();
                Camera.main.DOOrthoSize(dashCameraSize[(int)dashLevel], 0.5f).SetEase(Ease.OutCubic);
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

    #region Damaged
    IEnumerator Invincibility(float duration, float alphaValue)
    {
        UIManager.instance.bloodScreen.BlooeScreenLogic(0.25f, 0.75f);
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

        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(Camera.main.DOShakePosition(
                                        duration: 0.75f,
                                        new Vector3(0.3f, 0f, 0f),
                                        vibrato: 5,
                                        randomness: 90))
                                      .SetEase(Ease.OutQuad);

        StartCoroutine(Invincibility(0.8f, 0.5f));
        theStatus.hp -= damage;

        if (theStatus.hp <= 0)
        {
            Dead();
        }

        SFXController.instance.PlaySFX(clip); 

        UIManager.instance.heart.CheckHp(theStatus.hp);
    }

    void CameraSize(float x)
    {
        Camera.main.orthographicSize = x;
    }

    void Dead()
    {
        //TO-DO: Create Dead Logic

        StopAllCoroutines();

        Loading.LoadScene("Stage_01_Tutorial");
    }
    #endregion

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

        if (theStatus.maxHp <= theStatus.hp)
            return;

        theStatus.hp += value;

        UIManager.instance.heart.CheckHp(theStatus.hp);
    }
    #endregion

    public void StopAction()
    {
        canMove = false;
        onWalk = false;

        if (IncreaseDashLevelCoroutine != null)
            StopCoroutine(IncreaseDashLevelCoroutine);

        dashLevel = DashLevel.None;

        thePlayerAnimation.PlayDownhillAnimation(false);
        thePlayerAnimation.PlayJumpAnimation(false);
        thePlayerAnimation.PlaySlidingAnimation(false);
        thePlayerAnimation.PlayDashAnimation(false);
        thePlayerAnimation.PlayWalkAnimation(false);
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
