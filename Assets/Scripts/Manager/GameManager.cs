using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveValue
{
    static int _totalStageCount = 6;

    //public static int _deathCount = 0;
    public static int _totalScore = 0;
    public static int _hp = 3;
    public static int _maxHp = 3;
    public static int _nowSceneIndex = 0;

    public static ClearGradeSpace.ClearGrade[] BestGrades = new ClearGradeSpace.ClearGrade[_totalStageCount];
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] AudioClip _endClip;

    PlayerController theplayerController;
    UIManager theUIManager;
    FloorManager thefloorManager;
    StageManager theStageManager;
    AudioManager theAudioManager;

    public PlayerController PlayerControllerInstance { get { return theplayerController; } }
    public UIManager UIManagerInstance { get { return theUIManager; } }
    public FloorManager FloorManagerInstance { get { return thefloorManager; } }
    public StageManager StageManagerInstance { get { return theStageManager; } }
    public AudioManager AudioManagerInstance { get { return theAudioManager; } }

    bool _onOption;
    public bool _onResult;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);

        theplayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        theUIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        thefloorManager = GameObject.Find("FloorGroup").GetComponent<FloorManager>();
        theStageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        theAudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void Start()
    {
        theAudioManager.PlayBGM(StageManagerInstance._sceneIndex);
    }

    private void Update()
    {
        CheckOption();
        CheckCursor();
    }

    void CheckCursor()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.Locked;
    }

    void CheckOption()
    {
        if (!_onResult && Input.GetKeyDown(KeyCode.Escape))
        {
            if (_onOption)
                theUIManager.SetOption(false);
            else
                theUIManager.SetOption(true);

            _onOption = !_onOption;
        }
    }

    public void SaveTotalScore(int value)
    {
        SaveValue._totalScore = value;
    }

    public void SaveHp(int hp, int maxHp)
    {
        SaveValue._hp = hp;
        SaveValue._maxHp = maxHp;
    }

    public int GetSceneIndex()
    {
        return SaveValue._nowSceneIndex;
    }

    public void IncreaseSceneIndex(int index)
    {
        SaveValue._nowSceneIndex = index;
    }

    //public int GetDeathCount()
    //{
    //    return SaveValue._deathCount;
    //}

    //public void IncreaseDeathCount()
    //{
    //    SaveValue._deathCount++;
    //}

    public int LoadTotalScore()
    {
        return SaveValue._totalScore;
    }

    public int LoadHp()
    {
        return SaveValue._hp;
    }

    public int LoadMaxHp()
    {
        return SaveValue._maxHp;
    }

    public void GameStop()
    {
        theAudioManager.PauseAllSFXChannel();
        Time.timeScale = 0;
    }
    
    public void GameContinue()
    {
        theAudioManager.PlayAllSFXChannel();
        Time.timeScale = 1;
    }
}
