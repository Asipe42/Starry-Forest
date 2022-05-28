using UnityEngine;
using DG.Tweening;

public class HUD : MonoBehaviour
{
    [SerializeField] RectTransform heartBox;
    [SerializeField] RectTransform rsBox; // rank and score
    [SerializeField] RectTransform pdBox; // progress and dash;

    #region Show HUD UI
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
    #endregion

    #region Hide HUD UI
    public void HideHeartBox(float duration)
    {
        heartBox.DOAnchorPos(new Vector2(-400f, 0f), duration).SetEase(Ease.InQuad);
    }

    public void HideRSBox(float duration)
    {
        rsBox.DOAnchorPos(new Vector2(420f, 90f), duration).SetEase(Ease.InQuad);
    }

    public void HidePDBox(float duration)
    {
        pdBox.DOAnchorPos(new Vector2(0f, -200f), duration).SetEase(Ease.InQuad);
    }
    #endregion
}
