using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BGMController : MonoBehaviour
{
    public static BGMController instance;

    enum SceneState
    {
        Title = 0,
        Map,
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
        Initialize();
    }

    #region Initial Setting
    void Initialize()
    {
        instance = this;

        audioSource = GetComponent<AudioSource>();

        audioSource.volume = defaultVolume;
    }
    #endregion

    void Start()
    {
        StartCoroutine(WaitPlayBGM(this.delay));
    }

    /// <summary>
    /// volume�� duration�� ���� target��ŭ �����մϴ�.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="duration"></param>
    public void FadeVolume(float target = 1f, float duration = 1f)
    {
        audioSource.DOFade(target, duration);
        audioSource.Play();
    }

    /// <summary>
    /// FadeVolume�� delay�� ���Ŀ� �����մϴ�.
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    public IEnumerator WaitPlayBGM(float delay = 0f)
    {
        if (sceneState == SceneState.Title || sceneState == SceneState.Map)
        {
            yield return new WaitForSeconds(delay);
        }
        else if (sceneState == SceneState.Stage)
        {
            yield return new WaitUntil(() => !PlayerController.instance.onTutorial);
        }

        FadeVolume(this.target, this.duration);
    }
}
