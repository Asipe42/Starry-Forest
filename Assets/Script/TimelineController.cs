using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public static TimelineController instance;

    PlayableDirector playable;

    void Awake()
    {
        instance = this;
        playable = GetComponent<PlayableDirector>();
    }

    public void PauseTimeline()
    {
        playable.Pause();
    }

    public void ContinueTimeline()
    {
        playable.Play();
    }
}
