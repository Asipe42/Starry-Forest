using System.Collections;
using UnityEngine;

public class Reminisce : MonoBehaviour
{
    public void WaitInput()
    {
        StartCoroutine(WaitInputLogic());
    }

    IEnumerator WaitInputLogic()
    {
        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
        TimelineController.instance.ContinueTimeline();
    }
}