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
    public bool onJump;
    public bool onDownhill;
    public bool onSliding;
    public bool onDash;

    [Space]
    public bool hasDownhill = true;
    public bool onInvincibility = false;

    PlayerAnimation thePlayerAnimation;
    PlayerAudio thePlayerAudio;
    PlayerMovement thePlayerMovement;
    PlayerParticle thePlayerParticle;
    Status theStatus;
    SpriteRenderer spriteRenderer;

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

    void Update()
    {
        Debug.Log(dashLevel);
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

        onSliding = true;

        thePlayerAnimation.PlaySlidingAnimation(onSliding);
        thePlayerAudio.PlaySFX_Sliding(0);
        thePlayerParticle.PlayDust(onSliding);

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
                thePlayerParticle.PlayDust(onSliding);
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

        if (onDash)
            return;

        onDash = true;
        thePlayerAnimation.PlayDashAnimation(onDash);
        thePlayerAudio.PlaySFX_Dash(0f);

        StartCoroutine(IncreaseDashLevel());
        StartCoroutine(CancleDash());
    }

    IEnumerator CancleDash()
    {
        while (true)
        {
            if (Input.GetKeyUp(UseKeys.dashKey))
            {
                onDash = false;
                thePlayerAnimation.PlayDashAnimation(onDash);
                StartCoroutine(DecraseDashLevel());
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
        while (!onDash || DashLevel.None < dashLevel)
        {
            float value = 0;

            if (DashLevel.None < dashLevel)
            {
                while (value < 2)
                {
                    value += 2;
                    yield return new WaitForSeconds(1f);
                }

                if (DashLevel.None < dashLevel)
                    dashLevel--;
            }

            yield return null;
        }
    }
    #endregion

    IEnumerator Invincibility(float duration, float alphaValue)
    {
        UIManager.instance.theBlood.BloodEffect(0.8f);
        onInvincibility = true;
        spriteRenderer.color = new Color(1, 1, 1, alphaValue);

        yield return new WaitForSeconds(duration);

        onInvincibility = false;
        spriteRenderer.color = new Color(1, 1, 1, 1f);
    }

    public void OnDamaged(int damage)
    {
        if (onTutorial)
            return;

        if (onInvincibility)
            return;

        StartCoroutine(Invincibility(0.8f, 0.5f));
        theStatus.hp -= damage;

        UIManager.instance.theHp.CheckHp(theStatus.hp);
    }

    public void TakeItem()
    {
        thePlayerAudio.PlaySFX_TakeItem();
        thePlayerParticle.PlayTakeItem();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            onJump = false;
            onDownhill = false;
            hasDownhill = true;

            thePlayerAnimation.PlayJumpAnimation(false);
            thePlayerAnimation.PlayDownhillAnimation(false);
            thePlayerAnimation.PlayWalkAnimation(true);
        }
    }
}
