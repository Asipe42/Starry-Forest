using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BloodScreen : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Ease startEase;
    [SerializeField] Ease endEase;

    /// <summary>
    /// Ç÷Èç È¿°ú¸¦ Àç»ýÇÕ´Ï´Ù.
    /// </summary>
    /// <param name="startDuration"></param>
    /// <param name="endDuration"></param>
    public void BloodScreenEffect(float startDuration, float endDuration)
    {
        image.DOFade(1f, startDuration).SetEase(startEase)
             .OnComplete(() => 
             { 
                 image.DOFade(0f, endDuration).SetEase(endEase); 
             });
    }
}