using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Blood : MonoBehaviour
{
    [SerializeField] Image image;

    public void BloodEffect(float duration)
    {
        image.color = new Color(1f, 1f, 1f, 1f);
        image.DOColor(new Color(1f, 1f, 1f, 0f), duration).SetEase(Ease.InCirc);
    }
}
