using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Dash : MonoBehaviour
{
    [SerializeField] Image frameImage;
    [SerializeField] Image fillImage;

    [Space]
    [SerializeField] float[] fillValues;
    [SerializeField] float duration = 0.5f;

    void Awake()
    {
        PlayerController.DashAction -= FillDash;
        PlayerController.DashAction += FillDash;
    }

    void Update()
    {
        if (PlayerController.instance.onDash)
        {
            ChangeFrameColor();
        }
        else
        {
            ResetFrameColor();
        }
    }

    void FillDash(DashLevel dashLevel)
    {
        float targetFillGuage = fillValues[(int)dashLevel];

        DOVirtual.Float(fillImage.fillAmount, targetFillGuage, duration, DashGaugeFillValue);
    }

    void DashGaugeFillValue(float x)
    {
        fillImage.fillAmount = x;
    }

    void ChangeFrameColor()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(frameImage.DOColor(Color.yellow, 0.5f)
                .SetLoops(-1, LoopType.Yoyo))
                .Append(frameImage.DOFillAmount(0f, 0.25f)
                .SetEase(Ease.OutQuad)
                .SetLoops(-1, LoopType.Restart));
    }

    void ResetFrameColor()
    {
        frameImage.DOColor(Color.white, 0.5f);
    }
}
