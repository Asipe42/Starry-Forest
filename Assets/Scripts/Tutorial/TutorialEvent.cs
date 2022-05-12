using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

struct PlayerInfo
{
    public PlayerController pc;
    public PlayerAnimation pa;
    public Rigidbody2D rb;
}

public class TutorialEvent : MonoBehaviour
{
    public static event Action<bool> OnTutorialEvent;

    [SerializeField] Image panel;
    [SerializeField] SpriteRenderer guide;
    [SerializeField] ParticleSystem screenParticle;
    [SerializeField] string targetActionName;
    [SerializeField] float duration = 0.3f;
    [SerializeField] bool lastEvent;

    Dictionary<string, KeyCode> targetKey;

    AudioClip tutorialPopupClip;

    bool onEvent;

    void Awake()
    {
        tutorialPopupClip = Resources.Load<AudioClip>("Audio/SFX/SFX_TutorialPopup");

        targetKey = new Dictionary<string, KeyCode>();

        targetKey.Add("jump", UseKeys.jumpKey);
        targetKey.Add("sliding", UseKeys.SlidingKey);
        targetKey.Add("downhill", UseKeys.jumpKey);
        targetKey.Add("dash", UseKeys.dashKey);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (onEvent)
            return;

        if (collision.CompareTag("Player"))
        {
            onEvent = true;

            OnTutorialEvent.Invoke(false);

            PlayerInfo playerInfo;
            playerInfo.pc = collision.gameObject.GetComponent<PlayerController>();
            playerInfo.pa = collision.gameObject.GetComponent<PlayerAnimation>();
            playerInfo.rb = collision.gameObject.GetComponent<Rigidbody2D>();          

            playerInfo.pc.PermitAction(targetActionName);
            playerInfo.pc.onPause = true;
            playerInfo.pa.SetAnimationClipSpeed(0f);

            screenParticle.Pause();
            panel.DOFade(0.4f, duration);

            #region Exception Case
            if (targetActionName == "downhill")
            {
                playerInfo.rb.gravityScale = 0f;
                playerInfo.rb.velocity = new Vector2(playerInfo.rb.velocity.x, 0f);

                guide.gameObject.SetActive(true);
            }
            else if (targetActionName == "dash")
            {
                guide.gameObject.SetActive(true);
                guide.gameObject.GetComponent<Animator>().enabled = true;
            }
            else
            {
                guide.DOFade(1f, 1f).SetLoops(-1, LoopType.Yoyo);
            }
            #endregion

            StartCoroutine(WaitAction(targetActionName, playerInfo));
        }
    }

    IEnumerator WaitAction(string targetActionName, PlayerInfo playerInfo)
    {
        SFXController.instance.PlaySFX(tutorialPopupClip, 0, 1.5f);

        yield return new WaitUntil(() => Input.GetKeyDown(targetKey[targetActionName]));

        playerInfo.pc.PermitAction(targetActionName, false);
        playerInfo.pc.onPause = false;
        playerInfo.pa.SetAnimationClipSpeed(1f);

        screenParticle.Play();
        panel.DOFade(0, duration);

        #region Exception Case
        if (targetActionName == "downhill")
        {
            guide.gameObject.SetActive(false);
        }
        else if (targetActionName == "dash")
        {
            guide.gameObject.SetActive(false);
        }
        else
        {
            guide.DOKill();
            guide.DOFade(0f, 0.25f);
        }
        #endregion

        OnTutorialEvent.Invoke(true);

        if (lastEvent)
        {
            playerInfo.pc.onTutorial = false;
            playerInfo.pc.PermitEveryAction(true);

            StartCoroutine(UIManager.instance.ShowHUD(0.5f));
            StartCoroutine(UIManager.instance.ShowSign(0.5f, 1));
        }
    }
}