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
        float duration = 1f;

        UIManager.instance.resultSign.ShowResultSign("완주 성공");
        yield return new WaitUntil(() => UIManager.instance.resultSign.endDirecting);

        UIManager.instance.fadeScreen.FadeScreenEffect(1, duration);
        yield return new WaitForSeconds(duration * 2);

        if (lastFloorState == LastFloorState.Normal)
        {
            PlayResultDirecting();
        }

        if (lastFloorState == LastFloorState.Tutorial)
        {
            LoadMapScene(stageTemplate.currentStageIndex + 1);
        }
    }

    void PlayResultDirecting()
    {
        UIManager.instance.result.PlayResultDirecting();
    }

    void LoadMapScene(int index)
    {
        GameManager.UnlockStage(index);
        Loading.LoadScene("Map");
    }
    #endregion
}
