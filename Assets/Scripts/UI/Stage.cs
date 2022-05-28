using UnityEngine;
using DG.Tweening;

public class Stage : MonoBehaviour
{
    [SerializeField] RectTransform stageInfo;
    [SerializeField] RectTransform startGuide;
    [SerializeField] RectTransform albumGuide;

    #region Show Stage UI
    public void ShowStageInfo(float duration)
    {
        stageInfo.DOAnchorPos(new Vector2(0f, 0f), duration).SetEase(Ease.OutQuad);
    }

    public void ShowStartGuide(float duration)
    {
        startGuide.DOAnchorPos(new Vector2(60, 60f), duration).SetEase(Ease.OutQuad);
    }

    public void ShowAlbumGuide(float duration)
    {
        albumGuide.DOAnchorPos(new Vector2(-60f, 60f), duration).SetEase(Ease.OutQuad);
    }
    #endregion

    #region Hide Stage UI
    public void HideStageInfo(float duration)
    {
        stageInfo.DOAnchorPos(new Vector2(-400f, 0f), duration).SetEase(Ease.InQuad);
    }

    public void HideStartGuide(float duration)
    {
        startGuide.DOAnchorPos(new Vector2(-400f, 60f), duration).SetEase(Ease.InQuad);
    }

    public void HideAlbumGuide(float duration)
    {
        albumGuide.DOAnchorPos(new Vector2(400f, -60f), duration).SetEase(Ease.InQuad);
    }
    #endregion
}
