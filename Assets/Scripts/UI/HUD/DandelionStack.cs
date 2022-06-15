using UnityEngine;
using TMPro;
using DG.Tweening;

public class DandelionStack : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dandelionCountText;

    public void CheckDandelionCount()
    {
        dandelionCountText.text = "X " + PlayerController.instance.flyCount;

        var sequence = DOTween.Sequence();

        sequence.Append(dandelionCountText.transform.DOShakeScale(0.5f))
                .AppendCallback(() => dandelionCountText.transform.localScale = Vector3.one);
    }
}
