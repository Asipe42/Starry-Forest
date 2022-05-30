using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] SignTemplate[] signTemplate; // 0: Tutorial, 1: Normal
    [SerializeField] Image panel;

    #region HUD Components
    public HUD hud { get; private set; }
    public Heart heart { get; private set; }
    public Score score { get; private set; }
    public Rank rank { get; private set; }
    public Dash dash { get; private set; }
    public ProgressBar progressBar { get; private set; }
    #endregion

    #region Popup Components
    public PauseMenu pauseMenu { get; private set; }
    public Setting setting { get; private set; }
    public Sign sign { get; private set; }
    public Result result { get; private set; }
    public Goal goal { get; private set; }
    #endregion

    #region ScreenEffect Components
    public BloodScreen bloodScreen { get; private set; }
    public FadeScreen fadeScreen { get; private set; }
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

        pauseMenu = GameObject.FindObjectOfType<PauseMenu>();
        setting = GameObject.FindObjectOfType<Setting>();
        sign = GameObject.FindObjectOfType<Sign>();
        result = GameObject.FindObjectOfType<Result>();
        goal = GameObject.FindObjectOfType<Goal>();

        bloodScreen = GameObject.FindObjectOfType<BloodScreen>();
        fadeScreen = GameObject.FindObjectOfType<FadeScreen>();
    }

    void Start()
    {
        StartCoroutine(ShowSign(1f, 0));
    }

    void Update()
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

    public IEnumerator ShowSign(float delay, int index)
    {
        yield return new WaitForSeconds(delay);

        sign.Initialize(signTemplate[index]);
        sign.Popup();
    }

    public void HideOption()
    {
        onOption = false;
        BGMController.instance.FadeVolume(0.5f);

        pauseMenu.SetActivation(onOption);
    }

    public void ShowOption()
    {
        onOption = true;
        BGMController.instance.FadeVolume(0f);

        pauseMenu.SetActivation(onOption);
    }

    public void ShowSetting(bool state)
    {
        setting.SetActivation(state);
    }

    public void DarkenScreen(float alpha, float duration)
    {
        panel.DOFade(alpha, duration);
    }
}
