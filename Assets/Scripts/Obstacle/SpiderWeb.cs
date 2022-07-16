using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;
using DG.Tweening;

public class SpiderWeb : Obstacle
{
    [SerializeField] AnimationCurve blinkCurve;
    [SerializeField] Light2D light;
    [SerializeField] float scaleDuration;
    [SerializeField] float blinkDuration;

    float timer;

    void Awake()
    {
        GetAudioClip();
    }

    void Start()
    {
        PlaySpiderWebAnimation();
    }

    void Update()
    {
        PlayBlinkEffect();
    }

    #region Initial Setting
    void GetAudioClip()
    {
        base.hitClip = Resources.Load<AudioClip>("Aduio/SFX/SFX_SpiderWeb");
    }
    #endregion

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            playerController.OnDebuff(DebuffType.Web);
        }
    }

    void PlaySpiderWebAnimation()
    {
        transform.DOScale(1.1f, scaleDuration).SetEase(Ease.OutQuad).SetLoops(1, LoopType.Yoyo).Play();
    }

    void PlayBlinkEffect()
    {
        AddTime();

        float targetIntensity = blinkCurve.Evaluate(timer);
        light.intensity = targetIntensity;
    }

    void AddTime()
    {
        timer += Time.deltaTime;

        if (timer >= blinkDuration)
        {
            timer = 0;
        }
    }
}
