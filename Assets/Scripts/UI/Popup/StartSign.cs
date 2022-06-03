using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartSign : MonoBehaviour
{
    [SerializeField] SignTemplate[] signTemplate;
    [SerializeField] RectTransform box;
    [SerializeField] Text text;
    [SerializeField] float delay = 4f;
    [SerializeField] float defaultPosY = 900f;
    [SerializeField] float targetPosY = 350f;
    [SerializeField] float duration = 3f;
    [SerializeField] Ease startEase = Ease.OutBounce;
    [SerializeField] Ease endEase;

    AudioClip popupClip;

    void Awake()
    {
        Initialize();
        GetAudioClip();
    }

    void Start()
    {
        StartCoroutine(ShowStartSign(1f, 0));
    }

    #region Initial Setting
    void Initialize(int index = 0)
    {
        text.text = signTemplate[index].message;
        text.fontSize = signTemplate[index].fontSize;
    }

    void GetAudioClip()
    {
        popupClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Popup");
    }
    #endregion


    public IEnumerator ShowStartSign(float delay, int index)
    {
        yield return new WaitForSeconds(delay);

        Initialize(index);
        Popup();
    }

    void Popup()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(box.DOAnchorPosY(targetPosY, duration).SetEase(startEase))
                .OnStart(() => SFXController.instance.PlaySFX(popupClip, 0.5f, 1f, 1f))
                .AppendInterval(delay)
                .OnComplete(() => box.DOAnchorPosY(defaultPosY, duration / 2f).SetEase(endEase));
    }
}
