using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public static TimelineController instance;

    PlayableDirector playable;

    void Awake()
    {
        Initialize();
    }

    #region Initial Setting
    void Initialize()
    {
        instance = this;

        playable = GetComponent<PlayableDirector>();
    }
    #endregion

    /// <summary>
    /// Timeline을 정지합니다.
    /// </summary>
    public void PauseTimeline()
    {
        playable.Pause();
    }

    /// <summary>
    /// Timeline을 재개합니다.
    /// </summary>
    public void ContinueTimeline()
    {
        playable.Play();
    }
}
