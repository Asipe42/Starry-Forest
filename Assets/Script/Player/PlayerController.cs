using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] CapsuleCollider2D defaultCollider;
    [SerializeField] CapsuleCollider2D slidingCollider;

    PlayerAnimation thePlayerAnimation;
    PlayerAudio thePlayerAudio;
    PlayerMovement thePlayerMovement;
    PlayerParticle thePlayerParticle;
    InputManager theInputManager;
    Status theStatus;
    SpriteRenderer spriteRenderer;

    [HideInInspector] public bool onJump;
    [HideInInspector] public bool onDownhill;
    [HideInInspector] public bool onSliding;

    bool onInvincibility = false;
    bool canDownhill = true;

    void Awake()
    {
        instance = this;

        thePlayerAnimation = GetComponent<PlayerAnimation>();
        thePlayerAudio = GetComponent<PlayerAudio>();
        thePlayerMovement = GetComponent<PlayerMovement>();
        thePlayerParticle = GetComponent<PlayerParticle>();
        theInputManager = GetComponent<InputManager>();
        theStatus = GetComponent<Status>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Jump()
    {
        onJump = true;

        thePlayerMovement.Movement_Jump();
        thePlayerAnimation.PlayJumpAnimation(true);
        thePlayerAudio.PlaySFX_Jump(0);
    }

    public void Downhill()
    {
        if (onDownhill)
            return;

        if (onSliding)
            return;

        if (!canDownhill)
            return;

        onDownhill = true;
        canDownhill = false;

        thePlayerAnimation.PlayJumpAnimation(false);
        thePlayerMovement.Movement_Downhill(true);
        thePlayerAnimation.PlayDownhillAnimation(onDownhill);
        thePlayerAudio.PlaySFX_Downhill(0);

        StartCoroutine(CancleDownhill());
    }

    public void Sliding()
    {
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

    IEnumerator CancleDownhill()
    {
        while (onDownhill)
        {
            if (Input.GetKeyUp(theInputManager.jumpKey))
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

    IEnumerator CancleSliding()
    {
        while (onSliding)
        {
            if (Input.GetKeyUp(theInputManager.SlidingKey))
            {
                onSliding = false;

                defaultCollider.enabled = true;
                slidingCollider.enabled = false;

                thePlayerAnimation.PlaySlidingAnimation(onSliding);
                thePlayerParticle.PlayDust(onSliding);
            }

            yield return null;
        }

        // TODO: Play Dust Effect

        yield return null;
    }

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
        if (onInvincibility)
            return;

        StartCoroutine(Invincibility(0.3f, 0.5f));
        theStatus.hp -= damage;

        UIManager.instance.theHp.CheckHp(theStatus.hp);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            onJump = false;
            onDownhill = false;
            canDownhill = true;

            thePlayerAnimation.PlayJumpAnimation(false);
            thePlayerAnimation.PlayDownhillAnimation(false);
            thePlayerAnimation.PlayWalkAnimation(true);
        }
    }
}
