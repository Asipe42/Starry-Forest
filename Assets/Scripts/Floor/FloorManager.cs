using System.Collections;
using UnityEngine;

public enum LastFloorState
{
    Tutorial,
    Normal,
    Bornfire
}

public class FloorManager : MonoBehaviour
{
    [SerializeField] FloorGenerator theFloorGenerator;
    [SerializeField] FloorTemplate[] floorSet;

    public int level = 0;
    public int nextStageIndex;

    void Start()
    {
        SetCandidate(level);
    }

    /// <summary>
    /// 플랫폼 난이도를 올린다.
    /// </summary>
    public void LevelUp()
    {
        level++;
        SetCandidate(level);
    }

    /// <summary>
    /// 플랫폼 그룹을 교체한다.
    /// </summary>
    /// <param name="index"></param>
    public void SetCandidate(int index)
    {
        if (index < 0 || index >= floorSet.Length)
        {
            Debug.LogWarning("out of range");
            return;
        }

        theFloorGenerator.candidate = floorSet[index].floor;
    }

    public IEnumerator EndStage(LastFloorState lastFloorState)
    {
        PlayerController.instance.reachLastFloor = true;

        switch (lastFloorState)
        {
            case LastFloorState.Tutorial:
                StartCoroutine(PlayerController.instance.StopAction(lastFloorState));
                yield return new WaitUntil(() => PlayerController.instance.endStage);
                UIManager.instance.goal.ShowGoal();
                yield return new WaitUntil(() => UIManager.instance.goal.endGoal);
                UIManager.instance.fadeScreen.Fade(1, 1f);
                yield return new WaitForSeconds(2f);
                Loading.LoadScene("Map");
                GameManager.instance.UnlockStage(nextStageIndex);
                break;
            case LastFloorState.Normal:
                StartCoroutine(PlayerController.instance.StopAction(lastFloorState));
                yield return new WaitUntil(() => PlayerController.instance.endStage);
                UIManager.instance.goal.ShowGoal();
                // 결과로 전환
                break;
            case LastFloorState.Bornfire:
                // 플레이어 멈춤
                // 기다렸다가
                // 결과
                // 모닥불 씬으로 이동
                break;
            default:
                Debug.LogError("argument is wrong");
                break;
        }
    }
}
