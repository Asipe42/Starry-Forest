using System.Collections;
using UnityEngine;

public enum LastFloorState
{
    Tutorial,
    Normal,
    Bornfire
}

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public StageTemplate stageTemplate;

    void Awake()
    {
        Initialize();
    }

    #region Initial Setting
    void Initialize()
    {
        instance = this;
    }
    #endregion

    #region Finish Stage
    public IEnumerator FinishStage(LastFloorState lastFloorState)
    {
        PlayerController.instance.reachLastFloor = true;

        StartCoroutine(WaitGoal(lastFloorState));
        yield return null;
    }

    IEnumerator WaitGoal(LastFloorState lastFloorState)
    {
        StartCoroutine(PlayerController.instance.GoalAction(lastFloorState));
        yield return new WaitUntil(() => PlayerController.instance.onGoal);

        StartCoroutine(PlayGoalDirecting(lastFloorState));
    }

    IEnumerator PlayGoalDirecting(LastFloorState lastFloorState)
    {
        UIManager.instance.resultSign.ShowResultSign("완주 성공");
        yield return new WaitUntil(() => UIManager.instance.resultSign.endDirecting);

        if (lastFloorState == LastFloorState.Normal || lastFloorState == LastFloorState.Bornfire)
        {
            Debug.Log(lastFloorState);
            PlayResultDirecting();
            yield return new WaitUntil(() => UIManager.instance.result.endDirecting);
            StartCoroutine(LoadMapScene(stageTemplate.currentStageIndex + 1, lastFloorState));
        }

        if (lastFloorState == LastFloorState.Tutorial)
        {
            StartCoroutine(LoadMapScene(stageTemplate.currentStageIndex + 1, lastFloorState));
        }
    }

    void PlayResultDirecting()
    {
        UIManager.instance.result.PlayResultDirecting();
    }

    IEnumerator LoadMapScene(int index, LastFloorState lastFloorState)
    {
        UIManager.instance.fadeScreen.FadeScreenEffect(1, 1f);
        yield return new WaitForSeconds(2f);

        GameManager.UnlockStage(index);

        if (lastFloorState == LastFloorState.Normal || lastFloorState == LastFloorState.Tutorial)
        {
            Loading.LoadScene("Map");
        }

        if (lastFloorState == LastFloorState.Bornfire)
        {
            Loading.LoadScene("Rest_" + stageTemplate.chapterIndex);
        }
    }
    #endregion
}
