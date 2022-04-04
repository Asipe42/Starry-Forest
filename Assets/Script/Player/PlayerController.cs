using System.Collections;
using UnityEngine;

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
    [SerializeField] CapsuleCollider2D defaultCollider;
    [SerializeField] CapsuleCollider2D slidingCollider;

    [Header("Dash")]
    public DashLevel dashLevel = DashLevel.None;
    [SerializeField] float[] dashTime;

    [Space]
    public bool onTutorial = true;
    public bool canJump;
    public bool canSliding;
    public bool canDownhill;
    public bool canDash;

    [Space]
    public bool onGround;
    public bool onWalk = true;
    public bool onJump;
    public bool onDownhill;
    public bool onSliding;
    public bool onDash;

    [Space]
    public bool hasDownhill = true;
    public bool hasDash = true;
    public bool onInvincibility = false;

    PlayerAnimation thePlayerAnimation;
    PlayerAudio thePlayerAudio;
    PlayerMovement thePlayerMovement;
    PlayerParticle thePlayerParticle;
    Status theStatus;
    SpriteRenderer spriteRenderer;

    IEnumerator IncreaseDashLevelCoroutine;
    IEnumerator DecreaseDashLevelCoroutine;

    void Awake()
    {
        instance = this;

        thePlayerAnimation = GetComponent<PlayerAnimation>();
        thePlayerAudio = GetComponent<PlayerAudio>();
        thePlayerMovement = GetComponent<PlayerMovement>();
        thePlayerParticle = GetComponent<PlayerParticle>();
        theStatus = GetComponent<Status>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    #region Jump
    public void Jump()
    {
        if (!canJump)
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
            if (Input.GetKeyUp(UseKeys.jumpKey))
            {
                onDownhill = false;

                thePlayerMovement.Movement_Downhill(onDownhill);
            }

            yield return null;
        }

        onDownhill = false;
        thePlayerMovement.Movement_Downhill(onDownhill);

        yield return null;
    }
    #endregion

    #region Sliding
    public void Sliding()
    {
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

        defaultCollider.enabled = false;
        slidingCollider.enabled = true;

        StartCoroutine(CancleSliding());
    }

    IEnumerator CancleSliding()
    {
        while (onSliding)
        {
            if (Input.GetKeyUp(UseKeys.SlidingKey))
            {
                onSliding = false;

                defaultCollider.enabled = true;
                slidingCollider.enabled = false;

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
        if (!canDash)
            return;

        if (!onGround)
            return;

        if (hasDash)
        {
            onDash = true;
            hasDash = false;

            thePlayerAnimation.PlayDashAnimation(onDash);
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
            if (Input.GetKeyUp(UseKeys.dashKey))
            {
                onDash = false;

                thePlayerAnimation.PlayDashAnimation(onDash);

                Invoke("ChargeDash", 0.5f);

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
            float value = 0;

            if (dashLevel < DashLevel.Max)
            {
                while (value < dashTime[(int)dashLevel])
                {
                    value += 1;
                    yield return new WaitForSeconds(1f);
                }

                thePlayerAudio.PlaySFX_DashLevelup(0, 1 + (float)dashLevel * 0.1f);

                if(dashLevel < DashLevel.Max)
                    dashLevel++;
            }

            yield return null;
        }
    }

    IEnumerator DecraseDashLevel()
    {
        while (!onDash)
        {
            if (DashLevel.None < dashLevel)
            {
                float value = 0;

                if (DashLevel.None < dashLevel)
                {
                    while (value < 1)
                    {
                        value += 1;
                        yield return new WaitForSeconds(0.5f);
                    }

                    if (DashLevel.None < dashLevel)
                        dashLevel--;
                }
            }
            yield return null;
        }
    }

    void ChargeDash()
    {
        hasDash = true;
    }
    #endregion

    IEnumerator Invincibility(float duration, float alphaValue)
    {
        UIManager.instance.theBlood.BloodEffect(1f);
        onInvincibility = true;
        spriteRenderer.color = new Color(1, 1, 1, alphaValue);

        yield return new WaitForSeconds(duration);

        onInvincibility = false;
        spriteRenderer.color = new Color(1, 1, 1, 1f);
    }

    public void OnDamaged(int damage, AudioClip clip)
    {
        if (onTutorial)
            return;

        if (onInvincibility)
            return;

        if (theStatus.hp <= 0)
        {
            Dead();
        }

        StartCoroutine(Invincibility(0.8f, 0.5f));
        theStatus.hp -= damage;

        if (!onTutorial)
        {
            AudioManager.instance.PlaySFX(clip);
        }    

        UIManager.instance.theHp.CheckHp(theStatus.hp);
    }

    void Dead()
    {
        // TO-DO: Create Dead Logic
        Loading.LoadScene("Stage_01");
    }

    public void TakeItem()
    {
        thePlayerAudio.PlaySFX_TakeItem();
        thePlayerParticle.PlayTakeItem();
    }

    public void Recover(int value)
    {
        if (theStatus.maxHp <= theStatus.hp)
            return;

        theStatus.hp += value;

        thePlayerAudio.PlaySFX_Recover();
        thePlayerParticle.PlayRecover();

        UIManager.instance.theHp.CheckHp(theStatus.hp);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if (!onSliding)
                onWalk = true;

            onJump = false;
            onDownhill = false;
            hasDownhill = true;

            thePlayerAnimation.PlayJumpAnimation(false);
            thePlayerAnimation.PlayDownhillAnimation(false);
            thePlayerAnimation.PlayWalkAnimation(true);
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
