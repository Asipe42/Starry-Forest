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
    /// �÷��� ���̵��� �ø���.
    /// </summary>
    public void LevelUp()
    {
        level++;
        SetCandidate(level);
    }

    /// <summary>
    /// �÷��� �׷��� ��ü�Ѵ�.
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
                // ����� ��ȯ
                break;
            case LastFloorState.Bornfire:
                // �÷��̾� ����
                // ��ٷȴٰ�
                // ���
                // ��ں� ������ �̵�
                break;
            default:
                Debug.LogError("argument is wrong");
                break;
        }
    }
}
