using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Dash : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Image frameImage;
    [SerializeField] Image fillImage;

    [Header("Values")]
    [SerializeField] float[] fillValues;
    [SerializeField] float duration = 0.5f;

    void Awake()
    {
        SubscribeEvent();
    }

    #region Initial Setting
    void SubscribeEvent()
    {
        PlayerController.DashEvent -= FillDash;
        PlayerController.DashEvent += FillDash;
    }
    #endregion

    void FillDash(DashLevel dashLevel)
    {
        Debug.Log((int)dashLevel);
        float targetFillGuage = fillValues[(int)dashLevel];

        DOVirtual.Float(fillImage.fillAmount, targetFillGuage, duration, DashGaugeFillValue);
    }

    void DashGaugeFillValue(float x)
    {
        fillImage.fillAmount = x;
    }
}
