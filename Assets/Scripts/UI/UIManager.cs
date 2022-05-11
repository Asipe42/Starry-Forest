using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] SignTemplate[] signTemplate; // 0: Tutorial, 1: Normal

    public GameObject UI_HUD;
    public GameObject UI_Popup;
    public GameObject UI_ScreenEffect;

    public Sign sign { get; private set; }
    public Heart heart { get; private set; }
    public BloodScreen bloodScreen { get; private set; }
    public Rank rank { get; private set; }
    public Score score { get; private set; }
    public Result result { get; private set; }
    public Option option { get; private set; }
    public Setting setting { get; private set; }

    BGMController theBGMController;

    bool onOption;

    void Awake()
    {
        instance = this;

        sign = GameObject.FindObjectOfType<Sign>();
        heart = GameObject.FindObjectOfType<Heart>();
        bloodScreen = GameObject.FindObjectOfType<BloodScreen>();
        rank = GameObject.FindObjectOfType<Rank>();
        score = GameObject.FindObjectOfType<Score>();
        result = GameObject.FindObjectOfType<Result>();
        option = GameObject.FindObjectOfType<Option>();
        setting = GameObject.FindObjectOfType<Setting>();

        theBGMController = FindObjectOfType<BGMController>();
    }

    void Start()
    {
        if (PlayerController.instance.onTutorial)
            UI_HUD.SetActive(false);

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

    public IEnumerator ShowHUD(float delay)
    {
        yield return new WaitForSeconds(delay);

        UI_HUD.SetActive(true);
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
        theBGMController.Fade(0.5f);

        option.SetActivation(onOption);
    }

    public void ShowOption()
    {
        onOption = true;
        theBGMController.Fade(0f);

        option.SetActivation(onOption);
    }

    public void ShowSetting(bool state)
    {
        setting.SetActivation(state);
    }


    public void ShowResult(bool state)
    {

    }
}
