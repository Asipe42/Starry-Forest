using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image fillImage;
    [SerializeField] GameObject speechBubble;
    [SerializeField] TextMeshProUGUI message;
    [SerializeField, Range(1, 7)] int grade = 7;
    [SerializeField] int turningPoint;
    [SerializeField] float gauge;
    [SerializeField] float gaugeMax;
    [SerializeField] float[] fillSpeed;

    [Header("Color")]
    [SerializeField] Color[] colors;
    public bool[] changedColor;
    public bool changedLevel;

    [Space]
    public static bool onLast;

    FloorManager floorManager;

    AudioSource audioSource;

    AudioClip notificationClip;
    AudioClip messageClip;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        floorManager = GameObject.FindObjectOfType<FloorManager>().GetComponent<FloorManager>();

        slider.maxValue = gaugeMax;

        notificationClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Notification");
        messageClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Message");

        ChangeColor(colors[colors.Length - 1]);
    }

    void Update()
    {
        if (!PlayerController.instance.onTutorial)
        {
            if (slider.value == slider.maxValue)
                return;

            FillGague();
            CheckGauge();
        }
    }

    void FillGague()
    {
        gauge += fillSpeed[(int)PlayerController.instance.dashLevel] * Time.deltaTime;
        slider.value = gauge;
    }

    void CheckGauge()
    {
        for (int i = 0; i < grade; i++)
        {
            if (changedColor[i])
                continue;

            if (slider.maxValue / (grade + 1) * (grade - i) <= slider.value)
            {
                if (i == turningPoint)
                {
                    if (!changedLevel)
                    {
                        changedLevel = true;
                        floorManager.LevelUp();
                    }
                }

                changedColor[i] = true;
                ChangeColor(colors[i]);
            }
        }

        if (gauge >= slider.maxValue)
        {
            gauge = slider.maxValue;
            
            if (!onLast)
            {
                onLast = true;

                ShowMessage();
            }
        }
    }

    void ChangeColor(Color nextColor)
    {
        fillImage.color = nextColor;
    }

    void ShowMessage()
    {
        SFXController.instance.PlaySFX(notificationClip, 0.25f);

        var seqeunce = DOTween.Sequence();

        speechBubble.transform.DOLocalRotate(new Vector3(0, 0, 360), 1.5f).SetEase(Ease.OutBounce, overshoot: 1.25f)
                              .OnComplete(() => message.DOText("거의 다 왔어요!", 0.5f)
                                                       .OnUpdate(() =>
                                                       {
                                                           if (!audioSource.isPlaying)
                                                           {
                                                               audioSource.pitch = 0.8f;
                                                               audioSource.clip = messageClip;
                                                               audioSource.Play();
                                                           }
                                                       })
                                                       .OnComplete(() =>
                                                       {
                                                           audioSource.Stop();
                                                           message.text = "";
                                                           message.DOText("조금만 힘을 내요!", 0.5f)
                                                                                .OnUpdate(() =>
                                                                                {
                                                                                    if (!audioSource.isPlaying)
                                                                                    {
                                                                                        audioSource.pitch = 0.8f;
                                                                                        audioSource.clip = messageClip;
                                                                                        audioSource.Play();
                                                                                    }
                                                                                })
                                                                                .OnComplete(() =>
                                                                                {
                                                                                    audioSource.Stop();
                                                                                    speechBubble.GetComponent<Image>().DOFade(0f, 1f);
                                                                                    message.DOFade(0f, 1f);
                                                                                });
                                                       }));
    }
}
