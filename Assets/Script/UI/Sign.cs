using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Sign : MonoBehaviour
{
    [SerializeField] Text stageText;
    [SerializeField] AudioClip popupClip;
    [SerializeField] string stageTitle;
    [SerializeField] float startDelay = 1.5f;
    [SerializeField] float endDelay = 4f;
    [SerializeField] float defaultPosY = 900f;
    [SerializeField] float targetPosY = 350f;
    [SerializeField] float duration = 3f;

    RectTransform rectTransform;
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rectTransform = GetComponent<RectTransform>();

        stageText.text = stageTitle;
        audioSource.clip = popupClip;
    }

    void Start()
    {
        Invoke("Popup", startDelay);
    }

    void Popup()
    {
        Sequence mySequence = DOTween.Sequence();

        rectTransform.DOAnchorPosY(targetPosY, duration).SetEase(Ease.OutBounce);
        audioSource.PlayDelayed(0.4f);
        rectTransform.DOAnchorPosY(defaultPosY, duration / 2).SetDelay(endDelay);
    }
}
