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

    void Start()
    {
        foreach (var audio in audioSource)
        {
            audio.volume = defaultVolume;
        }

        if (onFadeOut)
        {
            StartCoroutine(FadeOut());
        }

        // TODO: create another audio function

    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(fadeOutDelay);

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
}
