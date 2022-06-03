using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ProgressBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Slider slider;
    [SerializeField] Image gaugeImage;
    [SerializeField] GameObject speechBubbleOffset;
    [SerializeField] Image speechBubble;
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
                CheckGaugeFull();
                CheckGauge();
            }
        }
    }

    void FillGague()
    {
        if (gauge >= gaugeMax)
            return;

        gauge += fillSpeed[(int)PlayerController.instance.dashLevel] * Time.deltaTime;
        slider.value = gauge;
    }

    void CheckGaugeFull()
    {
        if (gauge >= gaugeMax)
        {
            if (!gaugeIsFull)
            {
                gaugeIsFull = true;
                fullGaugeEvent.Invoke(true);

                Cheer();
            }
        }
    }

    void CheckGauge()
    {
        if (gauge >= gaugeMax)
            return;

        for (int i = 0; i < grade; i++)
        {
            if (changedColor[i])
                continue;

            if (slider.maxValue / (grade + 1) * (grade - i) <= slider.value)
            {
                if (i == StageManager.instance.stageTemplate.levelUpTimeRate)
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
    }

    void ChangeColor(Color nextColor)
    {
        gaugeImage.color = nextColor;
    }

    void Cheer()
    {
        SFXController.instance.PlaySFX(notificationClip, 0.25f);

        PlayAppearAnimation();
    }

    void PlayAppearAnimation()
    {
        float insertTime = 1.5f;

        var sequence = DOTween.Sequence();

        sequence.Append(speechBubbleOffset.transform.DORotate(new Vector3(0f, 0f, 90f), 0.5f).SetEase(Ease.OutExpo))
                .InsertCallback(insertTime, () => PlayTypinganimation("거의 다 왔어요!"))
                .InsertCallback(insertTime * 2f, () => PlayTypinganimation("조금만 더 힘내세요!"))
                .OnComplete(() => StartCoroutine(PlayDisappearanimation(insertTime)));
    }

    void PlayTypinganimation(string message)
    {
        cheerText.text = "";

        cheerText.DOText(message, 0.5f)
                 .OnUpdate(() => PlayMessageClip())
                 .OnComplete(() => audioSource.Stop());
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

    IEnumerator PlayDisappearanimation(float delay)
    {
        yield return new WaitForSeconds(delay);

        var sequence = DOTween.Sequence();

        sequence.Append(speechBubble.DOFade(0f, 1f))
                .Append(cheerText.DOFade(0f, 1f));
    }
}
