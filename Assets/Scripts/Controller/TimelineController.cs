using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineController : MonoBehaviour
{
    public static TimelineController instant;

    PlayableDirector playable;

    void Awake()
    {
        instant = this;
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
