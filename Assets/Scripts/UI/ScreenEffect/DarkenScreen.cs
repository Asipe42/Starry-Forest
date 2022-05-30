using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DarkenScreen : MonoBehaviour
{
    [SerializeField] Image panel;

    /// <summary>
    /// 화면을 어둡게 만듭니다(플레이어 제외).
    /// </summary>
    /// <param name="target"></param>
    /// <param name="duration"></param>
    public void DarkenScreenEffect(float target, float duration)
    {
        panel.DOFade(target, duration);
    }
}
