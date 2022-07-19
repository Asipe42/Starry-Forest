using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Life : MonoBehaviour
{
    [SerializeField] Image panel;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] TextMeshProUGUI lifeValueText;
    [SerializeField] GameObject failUI;
    [SerializeField] float lifeupDuration;
    [SerializeField] float lifeDownDuration;
    [SerializeField] float changeColorDuration;
    [SerializeField] float appearDuration;
    [SerializeField] float disappearDuration;

    AudioClip lifeUpClip;
    AudioClip lifeDownClip;

    void Awake()
    {
        GetAudioClip();
    }

    void Start()
    {
        if (GameManager.changedLifeValue)
        {
            GameManager.changedLifeValue = false;

            PlayAppearAnimation(GameManager.lifeChangeState);

            GameManager.lifeChangeState = LifeChangeState.None;
        }
    }

    void GetAudioClip()
    {
        lifeUpClip = Resources.Load<AudioClip>("Audio/SFX/SFX_LifeUp");
        lifeDownClip = Resources.Load<AudioClip>("Audio/SFX/SFX_LifeDown");
    }

    void BlockInput(bool state)
    {
        GameManager.onInputLock = state;
    }

    void PlayLifeUpAnimation() 
    {
        Vector3 targetPosition = new Vector3(0, 5, 0);

        var sequence = DOTween.Sequence();

        sequence.Append(lifeValueText.transform.DOPunchPosition(targetPosition, lifeupDuration)).SetEase(Ease.OutQuad).OnComplete(() => lifeValueText.DOColor(Color.white, changeColorDuration))
                .Append(lifeValueText.DOColor(Color.green, changeColorDuration)).SetEase(Ease.OutQuad)
                .AppendCallback(() => lifeValueText.text = "" + GameManager.life)
                .AppendCallback(() => PlayLifeUpClip())
                .OnComplete(() => PlayDisappearAnimation());
    }

    void PlayLifeDownAnimation()
    {
        Vector3 targetPosition = new Vector3(0, -5, 0);

        var sequence = DOTween.Sequence();

        sequence.Append(lifeValueText.transform.DOPunchPosition(targetPosition, lifeDownDuration)).SetEase(Ease.OutQuad).OnComplete(() => lifeValueText.DOColor(Color.white, changeColorDuration))
                .Append(lifeValueText.DOColor(Color.red, changeColorDuration)).SetEase(Ease.OutQuad)
                .AppendCallback(() => lifeValueText.text = "" + GameManager.life)
                .AppendCallback(() => PlayLifeDownClip())
                .OnComplete(() => PlayDisappearAnimation());
    }

    void PlayLifeUpClip()
    {
        SFXController.instance.PlaySFX(lifeUpClip);
    }

    void PlayLifeDownClip()
    {
        SFXController.instance.PlaySFX(lifeDownClip);
    }

    public void PlayAppearAnimation(LifeChangeState state)
    {
        float intervalTime = 2f;

        var sequence = DOTween.Sequence();

        sequence.Append(panel.DOFade(0.8f, appearDuration)).SetEase(Ease.OutQuad)
                .Insert(intervalTime, icon.DOFade(1f, appearDuration)).SetEase(Ease.OutQuad)
                .Append(mainText.DOFade(1f, appearDuration)).SetEase(Ease.OutQuad)
                .Append(lifeValueText.DOFade(1f, appearDuration)).SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    if (state == LifeChangeState.Up)
                    {
                        PlayLifeUpAnimation();
                    }
                    else
                    {
                        PlayLifeDownAnimation();
                    }
                });
    }

    void PlayDisappearAnimation()
    {
        float intervalTime = 2f;

        var sequence = DOTween.Sequence();

        sequence.Append(panel.DOFade(0f, appearDuration)).SetEase(Ease.OutQuad)
                .Insert(intervalTime, icon.DOFade(0f, appearDuration)).SetEase(Ease.OutQuad)
                .Append(mainText.DOFade(0f, appearDuration)).SetEase(Ease.OutQuad)
                .Append(lifeValueText.DOFade(0f, appearDuration)).SetEase(Ease.OutQuad)
                .OnComplete(() => JudgeFail());
    }

    void JudgeFail()
    {
        if (GameManager.life <= 0)
        {
            LoadFailUI();
        }
        else
        {
            BlockInput(false);
        }
    }

    void LoadFailUI()
    {
        failUI.SetActive(true);
        StartCoroutine(failUI.GetComponent<Fail>().PlayFailDirection());
    }
}
