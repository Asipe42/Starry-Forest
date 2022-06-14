using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    #region HUD Components
    public HUD hud { get; private set; }
    public Heart heart { get; private set; }
    public Score score { get; private set; }
    public Rank rank { get; private set; }
    public Dash dash { get; private set; }
    public ProgressBar progressBar { get; private set; }
    public DandelionStack dandelionStack { get; private set; }
    #endregion

    #region Popup Components
    public PauseMenu pauseMenu { get; private set; }
    public Setting setting { get; private set; }
    public StartSign startSign { get; private set; }
    public Result result { get; private set; }
    public ResultSign resultSign { get; private set; }
    #endregion

    #region ScreenEffect Components
    public BloodScreen bloodScreen { get; private set; }
    public FadeScreen fadeScreen { get; private set; }
    public DarkenScreen darkenScreen { get; private set; }
    #endregion

    bool onOption;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        instance = this;

        hud = GameObject.FindObjectOfType<HUD>();
        heart = GameObject.FindObjectOfType<Heart>();
        score = GameObject.FindObjectOfType<Score>();
        rank = GameObject.FindObjectOfType<Rank>();
        dash = GameObject.FindObjectOfType<Dash>();
        progressBar = GameObject.FindObjectOfType<ProgressBar>();
        dandelionStack = GameObject.FindObjectOfType<DandelionStack>();

        pauseMenu = GameObject.FindObjectOfType<PauseMenu>();
        setting = GameObject.FindObjectOfType<Setting>();
        startSign = GameObject.FindObjectOfType<StartSign>();
        result = GameObject.FindObjectOfType<Result>();
        resultSign = GameObject.FindObjectOfType<ResultSign>();

        bloodScreen = GameObject.FindObjectOfType<BloodScreen>();
        fadeScreen = GameObject.FindObjectOfType<FadeScreen>();
        darkenScreen = GameObject.FindObjectOfType<DarkenScreen>();
    }

    void Update()
    {
        InputKey();
    }

    void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PlayerController.instance.onTutorial)
                return;

            if (onOption)
            {
                HideOption();
            }
            else
            {
                ShowOption();
            }
        }
    }

    public void HideOption()
    {
        onOption = false;

        pauseMenu.SetActivation(onOption);
    }

    public void ShowOption()
    {
        onOption = true;

        pauseMenu.SetActivation(onOption);
    }

    public void ShowSetting(bool state)
    {
        setting.SetActivation(state);
    }
}