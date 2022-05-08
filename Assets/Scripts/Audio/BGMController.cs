using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BGMController : MonoBehaviour
{
    enum SceneState
    {
        Title = 0,
        Stage
    }

    [SerializeField] float defaultVolume = 0;
    [SerializeField] float delay;
    [SerializeField] float duration;
    [SerializeField] float target;
    [SerializeField] SceneState sceneState;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource.volume = defaultVolume;

        StartCoroutine(WaitPlayBGM(this.delay));
    }

    public void Fade(float target = 1f, float duration = 1f)
    {
        audioSource.DOFade(target, duration).SetDelay(delay);
        audioSource.Play();
    }

    public IEnumerator WaitPlayBGM(float delay = 0f)
    {
        if (sceneState == SceneState.Title)
        {
            yield return new WaitForSeconds(delay);
        }
        else if (sceneState == SceneState.Stage)
        {
            yield return new WaitUntil(() => !PlayerController.instance.onTutorial);
        }

        Fade(this.target, this.duration);
    }
}
