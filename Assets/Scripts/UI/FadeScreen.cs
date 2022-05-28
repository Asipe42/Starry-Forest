using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeScreen : MonoBehaviour
{
    public static event Action<bool> FadeEvent;

    [Header("Values")]
    [SerializeField] Image image;
    [SerializeField] float delay = 2f;
    [SerializeField] float duration = 0.8f;
    [SerializeField, Range(0, 1)] float target = 0f;

    SceneType sceneType;

    void Start()
    {
        Fade(this.target, this.duration, this.delay);
    }

    public void Fade(float target, float duration = 1f, float delay = 0f)
    {
        if (sceneType == SceneType.Title)
        {
            image.DOFade(target, duration).SetDelay(delay).OnComplete(() => 
            {
                FadeEvent.Invoke(true);
            });
        }
        else
        {
            image.DOFade(target, duration).SetDelay(delay);
        }
    }
}
