using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UI
{
    HUD = 0,
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

    public GameObject UI_HUD;
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
            Activate_Popup_Children(Children_Popup.Option, !onOption);
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

    public void Activate_HUD_Children(Children_HUD children, bool state = true)
    {
        if (!UI_HUD.activeSelf)
            return;

        switch (children)
        {
            case Children_HUD.Heart:
                theHeart.gameObject.SetActive(state);
                break;
            case Children_HUD.Blood:
                theBlood.gameObject.SetActive(state);
                break;
            case Children_HUD.Rank:
                theRank.gameObject.SetActive(state);
                break;
            case Children_HUD.Score:
                theScore.gameObject.SetActive(state);
                break;
        }
    }

    public void Activate_Popup_Children(Children_Popup children, bool state = true)
    {
        if (!UI_Popup.activeSelf)
            return;

        switch (children)
        {
            case Children_Popup.Result:
                theResult.gameObject.SetActive(state);
                StartCoroutine(theResult.ShowResult());
                break;
            case Children_Popup.Option:
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
                break;
            case Children_Popup.Setting:
                theSetting.gameObject.SetActive(state);
                break;
            default:
                break;
        }
    }
}
