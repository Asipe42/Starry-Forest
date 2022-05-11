using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BloodScreen : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Ease startEase;
    [SerializeField] Ease endEase;

    public void BlooeScreenLogic(float startDuration, float endDuration)
    {
        image.DOFade(1f, startDuration)
            .SetEase(startEase)
            .OnComplete(() => 
            { 
                image.DOFade(0f, endDuration).SetEase(endEase); 
            });
    }
}