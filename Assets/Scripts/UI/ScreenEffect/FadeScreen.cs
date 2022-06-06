using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeScreen : MonoBehaviour
{
    public static event Action<bool> fadeEvent;

    [Header("Values")]
    [SerializeField] Image image;
    [SerializeField] float delay = 2f;
    [SerializeField] float duration = 0.8f;
    [SerializeField, Range(0, 1)] float target = 0f;
    [SerializeField] SceneType sceneType = SceneType.InGame;

    void Start()
    {
        FadeScreenEffect(this.target, this.duration, this.delay);
    }

    /// <summary>
    /// 페이드 아웃(Fade-Out) 효과를 재생합니다.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="duration"></param>
    /// <param name="delay"></param>
    public void FadeScreenEffect(float target, float duration = 1f, float delay = 0f)
    {
        if (sceneType == SceneType.Title || sceneType == SceneType.Map)
        {
            image.DOFade(target, duration).SetDelay(delay).OnComplete(() => 
            {
                fadeEvent.Invoke(true);
            });
        }
        
        if (sceneType == SceneType.InGame)
        {
            image.DOFade(target, duration).SetDelay(delay);
        }

        if (sceneType == SceneType.Timeline)
        {
            ; // do nohitng
        }
    }

    /// <summary>
    /// Timeline 전용 함수
    /// </summary>
    public void Fade(bool state)
    {
        if (state)
            image.DOFade(0, 1f);
        else
            image.DOFade(1, 1f);
    }
}