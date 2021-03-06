using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Stage : MonoBehaviour
{
    [SerializeField] RectTransform stageInfo;
    [SerializeField] RectTransform startGuide;
    [SerializeField] RectTransform albumGuide;

    #region Show Stage UI
    public IEnumerator ShowEveryElements(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);
        ShowStageInfo(duration);
        ShowStartGuide(duration);
        ShowAlbumGuide(duration);
    }

    public void ShowStageInfo(float duration)
    {
        stageInfo?.DOAnchorPos(new Vector2(0f, 0f), duration).SetEase(Ease.OutExpo);
    }

    public void ShowStartGuide(float duration)
    {
        startGuide?.DOAnchorPos(new Vector2(60, 10), duration).SetEase(Ease.OutExpo);
    }

    public void ShowAlbumGuide(float duration)
    {
        albumGuide?.DOAnchorPos(new Vector2(-60f, 10), duration).SetEase(Ease.OutExpo);
    }
    #endregion

    #region Hide Stage UI
    public IEnumerator HideEveryElements(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);
        HideStageInfo(duration);
        HideStartGuide(duration);
        HideAlbumGuide(duration);
    }

    public void HideStageInfo(float duration)
    {
        stageInfo?.DOAnchorPos(new Vector2(-400f, 0f), duration).SetEase(Ease.InQuad);
    }

    public void HideStartGuide(float duration)
    {
        startGuide?.DOAnchorPos(new Vector2(-450, 10f), duration).SetEase(Ease.InQuad);
    }

    public void HideAlbumGuide(float duration)
    {
        albumGuide?.DOAnchorPos(new Vector2(450, 10f), duration).SetEase(Ease.InQuad);
    }
    #endregion
}
