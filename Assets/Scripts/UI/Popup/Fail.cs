using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Fail : MonoBehaviour
{
    [SerializeField] Image panel;
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] TextMeshProUGUI subText;
    [SerializeField] GameObject scrollTargetObject;
    [SerializeField] Transform scrollEndPosition;
    [SerializeField] Image dalDeadImage;
    [SerializeField] string mainMessage;
    [SerializeField] string subMessage;

    bool endTyping;
    bool endFade;
    bool endScroll;

    AudioSource audioSource;

    AudioClip typingClip;

    void Awake()
    {
        Initialize();
        GetAudioClip();
    }

    void Start()
    {
        AllocateAudioClip();
    }

    #region Initial Setting
    void Initialize()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = typingClip;
    }

    void GetAudioClip()
    {
        typingClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Typing");
    }

    void AllocateAudioClip()
    {
        audioSource.clip = typingClip;
    }
    #endregion

    public IEnumerator PlayFailDirection()
    {
        panel.DOFade(1f, 1f);
        BlockInput(true);

        PlayTypingAnimation(mainText, mainMessage);
        yield return new WaitUntil(() => endTyping);

        FadeOutObject(dalDeadImage);
        yield return new WaitUntil(() => endFade);

        ScrollImage(scrollTargetObject);
        yield return new WaitUntil(() => endScroll);

        endFade = false;
        subText.text = subMessage;
        FadeOutObject(subText);
        yield return new WaitUntil(() => endFade);

        yield return new WaitUntil(() => WaitAnyKey());
        GoTitle();
    }

    void BlockInput(bool state)
    {
        if (state)
        {
            GameManager.onInputLock = true;
        }
        else
        {
            GameManager.onInputLock = false;
        }
    }

    void PlayTypingAnimation(TextMeshProUGUI targetText, string message)
    {
        float typingDuration = 3f;

        targetText.DOText(message, typingDuration)
                    .OnUpdate(() => audioSource.Play())
                    .OnComplete(() =>
                    {
                        audioSource.Stop();
                        endTyping = true;
                    });
    }


    void FadeOutObject(Image image)
    {
        float fadeDuration = 1f;

        image.DOFade(1f, fadeDuration)
             .OnComplete(() => endFade = true);
    }

    void FadeOutObject(TextMeshProUGUI text)
    {
        float fadeDuration = 1f;

        text.DOFade(1f, fadeDuration)
             .OnComplete(() => endFade = true);
    }

    void ScrollImage(GameObject targetobject)
    {
        float scrollDuration = 1f;

        targetobject.transform.DOMoveX(scrollEndPosition.position.x, scrollDuration).SetEase(Ease.OutBounce)
                              .OnComplete(() => endScroll = true);
    }

    bool WaitAnyKey()
    {
        return Input.anyKey;
    }

    void GoTitle()
    {
        Loading.LoadScene("Title");
    }
}
