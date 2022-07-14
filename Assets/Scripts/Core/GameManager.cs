using UnityEngine;
using DG.Tweening;

public struct StageButtonInfo
{
    public bool isLockedStage;
    public Grade highGrade;
}

public class GameManager : MonoBehaviour
{
    public Texture2D[] cursorImg;

    public static GameManager instance;
    public static StageButtonInfo[] stageButtonInfo;
    public static int life = 3;
    public static int lastSelectedStageButtonIndex = 0;
    public static int stageIndex = 11;
    public static int currentChapterIndex;

    void Awake()
    {
        Initialize();
        InitializeStageButtonInfo();
        ChangeCursorImage();
    }

    void Start()
    {
        Cursor.visible = false;
    }

    #region Initial Setting
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

    void ChangeCursorImage()
    {
        Cursor.SetCursor(cursorImg[0], new Vector2(cursorImg[0].width / 2, cursorImg[1].height / 2), CursorMode.ForceSoftware);
    }
    #endregion

    #region Data Initialize
    public static void InitializeStageButtonInfo()
    {
        stageButtonInfo = new StageButtonInfo[stageIndex];

        for (int i = 0; i < stageIndex; i++)
        {
            stageButtonInfo[i].isLockedStage = true;
            stageButtonInfo[i].highGrade = Grade.None;
        }

        // TEST
        stageButtonInfo[0].isLockedStage = false;
        stageButtonInfo[1].isLockedStage = false;
        stageButtonInfo[2].isLockedStage = false;
        stageButtonInfo[3].isLockedStage = false;
        stageButtonInfo[4].isLockedStage = false;

        stageButtonInfo[5].isLockedStage = false;
        stageButtonInfo[6].isLockedStage = false;
        stageButtonInfo[7].isLockedStage = false;
        stageButtonInfo[8].isLockedStage = false;
        stageButtonInfo[9].isLockedStage = false;
    }

    public static void InitializeLifeCount(int value = 3)
    {
        life = value;
    }
    #endregion

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
