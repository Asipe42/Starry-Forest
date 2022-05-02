using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BGMController : MonoBehaviour
{
    [SerializeField] float defaultVolume = 0;
    [SerializeField] float delay;
    [SerializeField] float duration;
    [SerializeField] float target;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource.volume = defaultVolume;

        StartCoroutine(WaitPlayBGM());
    }

    public void Fade(float target = 1f, float duration = 1f, float delay = 0f)
    {
        audioSource.DOFade(target, duration).SetDelay(delay);
        audioSource.Play();
    }

    public IEnumerator WaitPlayBGM()
    {
        yield return new WaitUntil(() => !PlayerController.instance.onTutorial);

        Fade(this.target, this.duration, this.delay);
    }
}
