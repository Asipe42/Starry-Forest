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
    [SerializeField] Image panel;
    [SerializeField] SpriteRenderer guide;
    [SerializeField] string targetActionName;
    [SerializeField] float duration = 0.3f;
    [SerializeField] bool lastEvent;

    ParticleSystem screenParticle;

    Dictionary<string, KeyCode> targetKey;

    AudioClip tutorialPopupClip;

    bool onEvent;

    public static event Action<bool> tutorialEvent;

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        AllocateScreenParticle();
    }

    #region Initial Setting
    void Initialize()
    {
        tutorialPopupClip = Resources.Load<AudioClip>("Audio/SFX/SFX_TutorialPopup");

        targetKey = new Dictionary<string, KeyCode>();

        targetKey.Add("jump", UseKeys.jumpKey);
        targetKey.Add("sliding", UseKeys.slidingKey);
        targetKey.Add("downhill", UseKeys.jumpKey);
        targetKey.Add("dash", UseKeys.dashKey);
        targetKey.Add("fly", UseKeys.specialKey);
    }
    #endregion

    void AllocateScreenParticle()
    {
        screenParticle = GameObject.FindObjectOfType<Scroll>().transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (onEvent)
            return;

        if (collision.CompareTag("Player"))
        {
            onEvent = true;

            tutorialEvent.Invoke(false);

            PlayerInfo playerInfo;
            playerInfo.pc = collision.gameObject.GetComponent<PlayerController>();
            playerInfo.pa = collision.gameObject.GetComponent<PlayerAnimation>();
            playerInfo.rb = collision.gameObject.GetComponent<Rigidbody2D>();          

            playerInfo.pc.PermitAction(targetActionName);
            playerInfo.pc.onPause = true;
            playerInfo.pa.SetAnimationSpeed(0f);

            screenParticle.Pause();
            panel.DOFade(0.4f, duration);

            #region Exception Case
            if (targetActionName == "downhill")
            {
                RemoveExternalForce(playerInfo);
                guide.gameObject.SetActive(true);
            }
            else if (targetActionName == "dash")
            {
                guide.gameObject.SetActive(true);
            }
            else if (targetActionName == "fly")
            {
                RemoveExternalForce(playerInfo);
                guide.DOFade(1f, 1f).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                guide.DOFade(1f, 1f).SetLoops(-1, LoopType.Yoyo);
            }
            #endregion

            StartCoroutine(WaitAction(targetActionName, playerInfo));
        }
    }

    void RemoveExternalForce(PlayerInfo playerInfo)
    {
        playerInfo.rb.gravityScale = 0f;
        playerInfo.rb.velocity = new Vector2(playerInfo.rb.velocity.x, 0f);
    }

    IEnumerator WaitAction(string targetActionName, PlayerInfo playerInfo)
    {
        SFXController.instance.PlaySFX(tutorialPopupClip, 0, 1.5f);

        yield return new WaitUntil(() => Input.GetKeyDown(targetKey[targetActionName]));

        playerInfo.pc.PermitAction(targetActionName, false);
        playerInfo.pc.onPause = false;
        playerInfo.pa.SetAnimationSpeed(1f);

        screenParticle.Play();
        panel.DOFade(0, duration);

        #region Exception Case
        if (targetActionName == "jump")
        {
            UIManager.instance.hud.ShowHeartBox(1f);
            guide.DOKill();
            guide.DOFade(0f, 0.25f);
        }
        else if (targetActionName == "downhill")
        {
            guide.gameObject.SetActive(false);
        }
        else if (targetActionName == "dash")
        {
            UIManager.instance.hud.ShowPDBox(1f);
            guide.gameObject.SetActive(false);
        }
        else
        {
            guide.DOKill();
            guide.DOFade(0f, 0.25f);
        }
        #endregion

        tutorialEvent.Invoke(true);

        if (lastEvent)
        {
            playerInfo.pc.onTutorial = false;
            playerInfo.pc.PermitEveryAction(true);

            UIManager.instance.hud.ShowHeartBox(1f);
            UIManager.instance.hud.ShowPDBox(1f);
            UIManager.instance.hud.ShowRSBox(1f);

            StartCoroutine(UIManager.instance.startSign.ShowStartSign(0.5f, 1));
        }
    }
}
