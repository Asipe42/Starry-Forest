using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Dash : MonoBehaviour
{
    [SerializeField] Image fillImage;

    [Space]
    [SerializeField] float[] fillValues;
    [SerializeField] float duration = 0.5f;

    void Awake()
    {
        PlayerController.DashAction -= FillDash;
        PlayerController.DashAction += FillDash;
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
}
