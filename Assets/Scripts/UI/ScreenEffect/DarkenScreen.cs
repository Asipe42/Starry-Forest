using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DarkenScreen : MonoBehaviour
{
    [SerializeField] Image panel;

    /// <summary>
    /// ȭ���� ��Ӱ� ����ϴ�(�÷��̾� ����).
    /// </summary>
    /// <param name="target"></param>
    /// <param name="duration"></param>
    public void DarkenScreenEffect(float target, float duration)
    {
        panel.DOFade(target, duration);
    }
}
