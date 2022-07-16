using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StrawberryStack : MonoBehaviour
{
    [SerializeField] float upDuration = 0.5f;
    [SerializeField] float downDuration = 0.3f;
    [SerializeField] Image[] icons;

    /// <summary>
    /// state == true: get, state == false: use
    /// </summary>
    /// <param name="currentStack"></param>
    /// <param name="state"></param>
    public void UpdateIcon(int currentStack, bool state) // true: Get , false: Use
    {
        PlayIconAnimation(icons[currentStack], state);
    }

    void PlayIconAnimation(Image target, bool state) // true: Up, false: End
    {
        if (state)
        {
            target.transform.DOScale(Vector3.one, upDuration).SetEase(Ease.OutBounce);
        }
        else
        {
            target.transform.DOScale(Vector3.zero, downDuration).SetEase(Ease.InBounce);
        }
    }
}
