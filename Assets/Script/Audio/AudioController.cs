using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioSource[] audioSource;

    [Header("Fade")]
    [SerializeField] float defaultVolume = 0;
    [SerializeField] float fadeOutDelay;
    [SerializeField] float fadeOutSpeed;
    [SerializeField] float fadeOutCooltime;
    [SerializeField] float fadeOutTargetVolume;
    [SerializeField] bool onFadeOut;
    [SerializeField] bool onBGM;

    void Start()
    {
        foreach (var audio in audioSource)
        {
            audio.volume = defaultVolume;
        }

        if (onFadeOut)
        {
            StartCoroutine(FadeOut(fadeOutDelay));
        }

        if (onBGM)
        {
            StartCoroutine(PlayBGM());
        }

        // TODO: create another audio function
    }

    public IEnumerator FadeOut(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var audio in audioSource)
        {
            if (!audio.isPlaying)
                audio.Play();

            while (audio.volume <= fadeOutTargetVolume)
            {
                audio.volume += fadeOutSpeed * Time.deltaTime;
                yield return new WaitForSeconds(fadeOutCooltime);
            }
        }
    }

    public IEnumerator PlayBGM()
    {
        yield return new WaitUntil(() => !PlayerController.instance.onTutorial);

        StartCoroutine(FadeOut(0));
    }
}
