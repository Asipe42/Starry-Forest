using UnityEngine;
using DG.Tweening;

public class HUD : MonoBehaviour
{
    [SerializeField] RectTransform heartBox;
    [SerializeField] RectTransform rsBox; // rank and score
    [SerializeField] RectTransform pdBox; // progress and dash;

    public void ShowHeartBox(float duration)
    {
        heartBox.DOAnchorPos(new Vector2(0f, 0f), duration).SetEase(Ease.OutQuad);
    }
    
    public void ShowRSBox(float duration)
    {
        rsBox.DOAnchorPos(new Vector2(0, 90f), duration).SetEase(Ease.OutQuad);
    }

    public void ShowPDBox(float duration)
    {
        pdBox.DOAnchorPos(new Vector2(0f, 0f), duration).SetEase(Ease.OutQuad);
    }
}
