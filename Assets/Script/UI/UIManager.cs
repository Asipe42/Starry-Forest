using UnityEngine;
using DG.Tweening;

public enum UI
{
    HUD = 0,
    Effect,
    Popup
}

public enum Children_HUD
{
    Heart = 0,
    Blood,
    Rank,
    Score,
    
}

public enum Children_Popup
{
    Result = 0,
    Option,
    Setting,
}

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] DOTweenAnimation optionPanelAnim;

    [Space]
    public GameObject UI_HUD;
    public GameObject UI_Effect;
    public GameObject UI_Popup;

    [Space]
    public Heart theHeart;
    public Blood theBlood;
    public Rank theRank;
    public Score theScore;
    public Result theResult;
    public Option theOption;
    public Setting theSetting;

    void Awake()
    {
        instance = this;

        theHeart = GameObject.FindObjectOfType<Heart>().GetComponent<Heart>();
        theBlood = GameObject.FindObjectOfType<Blood>().GetComponent<Blood>();
        theRank = GameObject.FindObjectOfType<Rank>().GetComponent<Rank>();
        theScore =  GameObject.FindObjectOfType<Score>().GetComponent<Score>();
        theResult = GameObject.FindObjectOfType<Result>().GetComponent<Result>();
        theOption = GameObject.FindObjectOfType<Option>().GetComponent<Option>();
        theSetting = GameObject.FindObjectOfType<Setting>().GetComponent<Setting>();
    }

    void Start()
    {
        if (PlayerController.instance.onTutorial)
        {
            ActivateUI(UI.HUD, false);
            ActivateUI(UI.Popup, false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool onOption = theOption.gameObject.activeSelf;
            ShowOption(!onOption);
        }
    }

    public void ActivateUI(UI ui, bool state = true)
    {
        switch (ui)
        {
            case UI.HUD:
                UI_HUD.SetActive(state);
                break;
            case UI.Popup:
                UI_Popup.SetActive(state);
                break;
            default:
                break;
        }
    }

    public void ShowOption(bool state)
    {
        optionPanelAnim.DOPlay();

        if (state)
        {
            InputManager.instance.onLock = true;
            Time.timeScale = 0f;
        }
        else
        {
            InputManager.instance.onLock = false;
            Time.timeScale = 1f;
        }

        theOption.gameObject.SetActive(state);
    }

    public void ShowResult(bool state)
    {
        theResult.gameObject.SetActive(state);
    }
}
