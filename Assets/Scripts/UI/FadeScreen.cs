using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] Menu menu;

    [Header("Values")]
    [SerializeField] Image image;
    [SerializeField] float delay = 2f;
    [SerializeField] float duration = 0.8f;
    [SerializeField, Range(0, 1)] float target = 0f;

    void Start()
    {
        Fade(this.target, this.duration, this.delay);
    }

    void Fade(float target, float duration = 1f, float delay = 0f)
    {
        image.DOFade(target, duration).SetDelay(delay);
    }
}
