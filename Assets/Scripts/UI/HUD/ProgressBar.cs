using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ProgressBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Slider slider;
    [SerializeField] Image gaugeImage;
    [SerializeField] GameObject speechBubble;
    [SerializeField] TextMeshProUGUI cheerText;
    [SerializeField] Color[] colors;
    public bool[] changedColor;
    public bool onLevelUp;

    [Header("Values")]
    [SerializeField, Range(1, 7)] int grade = 7;
    [SerializeField] float gauge;
    [SerializeField] float gaugeMax;
    [SerializeField] float[] fillSpeed;

    bool canProgress = true;
    bool gaugeIsFull;

    AudioSource audioSource;

    AudioClip notificationClip;
    AudioClip messageClip;

    public static event Action levelUpEvent;
    public static event Action<bool> fullGaugeEvent;

    void Awake()
    {  
        Initialize();
        GetAudioClip();
        SubscribeEvent();
        ChangeColor(colors[colors.Length - 1]);
    }

    #region Initial Setting
    void GetAudioClip()
    {
        notificationClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Notification");
        messageClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Message");
    }

    void Initialize()
    {
        audioSource = GetComponent<AudioSource>();

        slider.maxValue = gaugeMax;
    }

    void SubscribeEvent()
    {
        PlayerController.deadEvent -= SetCanProgress;
        PlayerController.deadEvent += SetCanProgress;
    }
    #endregion

    void SetCanProgress(bool state)
    {
        canProgress = state;
    }

    void Update()
    {
        if (canProgress)
        {
            if (!PlayerController.instance.onTutorial)
            {
                FillGague();
                CheckGauge();
            }
        }
    }

    void FillGague()
    {
        if (slider.value == slider.maxValue)
            return;

        gauge += fillSpeed[(int)PlayerController.instance.dashLevel] * Time.deltaTime;
        slider.value = gauge;
    }

    void CheckGauge()
    {
        if (slider.value == slider.maxValue)
            return;

        for (int i = 0; i < grade; i++)
        {
            if (changedColor[i])
                continue;

            if (slider.maxValue / (grade + 1) * (grade - i) <= slider.value)
            {
                if (i == FloorManager.instance.stageTemplate.levelUpTimeRate)
                {
                    if (!onLevelUp)
                    {
                        onLevelUp = true;
                        levelUpEvent.Invoke();
                    }
                }

                changedColor[i] = true;
                ChangeColor(colors[i]);
            }
        }

        if (gauge >= slider.maxValue)
        {
            gauge = slider.maxValue;
         
            if (!gaugeIsFull)
            {
                gaugeIsFull = true;
                fullGaugeEvent.Invoke(true);

                Cheer();
            }
        }
    }

    void ChangeColor(Color nextColor)
    {
        gaugeImage.color = nextColor;
    }

    void Cheer()
    {
        SFXController.instance.PlaySFX(notificationClip, 0.25f);

        var appearSequence = DOTween.Sequence();
        var disappearSequence = DOTween.Sequence();

        disappearSequence.Pause();

        appearSequence.Append(speechBubble.transform.DOLocalRotate(new Vector3(0f, 0f, 360f), 1.5f).SetEase(Ease.OutBounce, overshoot: 1.25f))
                .Insert(2f, cheerText.DOText("거의 다 왔어요!", 0.75f))
                                     .OnUpdate(() => PlayMessageClip())
                                     .OnComplete(() => audioSource.Stop())
                .Insert(3f, cheerText.DOText("조금만 더 힘내세요!", 0.75f))
                                     .OnStart(() => cheerText.text = "")
                                     .OnUpdate(() => PlayMessageClip())
                                     .OnComplete(() => audioSource.Stop())
                .OnComplete(() =>
                {
                    disappearSequence.Restart();
                });

        disappearSequence.AppendCallback(() => audioSource.Stop())
                         .Append(speechBubble.GetComponent<Image>().DOFade(0f, 1.25f))
                         .Append(cheerText.DOFade(0f, 1.25f));
    }

    void PlayMessageClip()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.pitch = 0.8f;
            audioSource.clip = messageClip;
            audioSource.Play();
        }
    }
}
