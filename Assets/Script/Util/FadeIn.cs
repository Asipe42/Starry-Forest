using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class FadeIn : MonoBehaviour
{
    public bool title;

    [ShowIf("title")]
    [SerializeField] Menu menu;

    [Header("Values")]
    [SerializeField] Image image;
    [SerializeField] float delay = 2f;
    [SerializeField] float fadeSpeed = 0.8f;
    [SerializeField, Range(0, 1)] float limit = 0f;

    void Start()
    {
        StartCoroutine(FadeInCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        yield return new WaitForSeconds(delay);

        if (menu != null)
            menu.onEnable = true;

        Color _color = image.color;

        while (_color.a >= limit)
        {
            _color.a -= fadeSpeed * Time.deltaTime;
            image.color = _color;

            yield return null;
        }
    }
}
