using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioSource[] audioSource;

    [Header("Fade")]
    [SerializeField] float defaultVolume = 0;
    [SerializeField] float fadeDelay;
    [SerializeField] float fadeSpeed;
    [SerializeField] float fadeCooltime;
    [SerializeField] float fadeTargetVolume;
    [SerializeField] bool onFadeOut;
    [SerializeField] bool onFadeIn;
    [SerializeField] bool onBGM;

    Coroutine FadeOutCoroutine;
    Coroutine FadeInCoroutine;

    void Start()
    {
        foreach (var audio in audioSource)
        {
            audio.volume = defaultVolume;
        }

        if (onFadeOut)
        {
            FadeOut(fadeDelay);
        }

        if (onFadeIn)
        {
            FadeIn(fadeDelay);
        }

        if (onBGM)
        {
            StartCoroutine(PlayBGM());
        }

        // TODO: create another audio function
    }

    public void FadeOut(float delay)
    {
        if (FadeInCoroutine != null)
            StopCoroutine(FadeInCoroutine);

        FadeOutCoroutine = StartCoroutine(FadeOutLogic(delay));
    }

    public void FadeIn(float delay)
    {
        if (FadeOutCoroutine != null)
            StopCoroutine(FadeOutCoroutine);

        FadeInCoroutine = StartCoroutine(FadeInLogic(delay));
    }

    IEnumerator FadeOutLogic(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var audio in audioSource)
        {
            if (!audio.isPlaying)
                audio.Play();

            while (audio.volume <= fadeTargetVolume)
            {
                audio.volume += fadeSpeed * Time.unscaledDeltaTime;
                yield return new WaitForSecondsRealtime(fadeCooltime);
            }
        }
    }

    IEnumerator FadeInLogic(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var audio in audioSource)
        {
            while (audio.volume > 0)
            {
                audio.volume -= fadeSpeed * Time.unscaledDeltaTime;
                yield return new WaitForSecondsRealtime(fadeCooltime);
            }
            
            audio.Pause();
        }
    }

    public IEnumerator PlayBGM()
    {
        yield return new WaitUntil(() => !PlayerController.instance.onTutorial);

        StartCoroutine(FadeOutLogic(1));
    }
}
