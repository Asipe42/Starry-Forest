using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Sign : MonoBehaviour
{
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
        popupClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Popup");
    }

    public void Popup()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(box.DOAnchorPosY(targetPosY, duration).SetEase(startEase))
                .OnStart(() => SFXController.instance.PlaySFX(popupClip, 0.5f, 1f, 1f))
                .AppendInterval(delay)
                .OnComplete(() => box.DOAnchorPosY(defaultPosY, duration / 2f).SetEase(endEase));
    }

    public void Initialize(SignTemplate signTemplate)
    {
        this.text.text = signTemplate.message;
        this.text.fontSize = signTemplate.fontSize;
    }
}
