using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField] Menu menu;
    [SerializeField] float delay = 2f;
    [SerializeField] float fadeSpeed = 0.8f;
    [SerializeField, Range(0, 1)] float limit = 0f;

    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

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
