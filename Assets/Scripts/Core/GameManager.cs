using UnityEngine;

public struct StageButtonInfo
{
    public bool isLockedStage;
    public Grade highGrade;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static StageButtonInfo[] stageButtonInfo;
    public static int life = 3;
    public static int lastSelectedStageButtonIndex = 0;
    public static int stageIndex = 11;

    void Awake()
    {
        Initialize();
        SetStageButtonInfo();
    }

    void Initialize()
    {
        #region Signlton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
        #endregion
    }

    public static void SetStageButtonInfo()
    {
        stageButtonInfo = new StageButtonInfo[stageIndex];

        for (int i = 0; i < stageIndex; i++)
        {
            stageButtonInfo[i].isLockedStage = true;
            stageButtonInfo[i].highGrade = Grade.None;
        }

        stageButtonInfo[0].isLockedStage = false;
    }

    public static void UnlockStage(int index, bool state = false)
    {
        stageButtonInfo[index].isLockedStage = state;
    }

    public static void RecordGrade(int index, Grade grade)
    {
        if (stageButtonInfo[index].highGrade == Grade.None)
        {
            stageButtonInfo[index].highGrade = grade;
        }

        if (stageButtonInfo[index].highGrade > grade)
        {
            stageButtonInfo[index].highGrade = grade;
        }
    }
}
