using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UI
{
    HUD = 0,
    Popup
}

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject UI_HUD;
    public GameObject UI_Popup;

    [Space]
    public Heart theHp;
    public Blood theBlood;
    public Rank theRank;
    public Score theScore;

    void Awake()
    {
        instance = this;

        theHp = GameObject.FindObjectOfType<Heart>().GetComponent<Heart>();
        theBlood = GameObject.FindObjectOfType<Blood>().GetComponent<Blood>();
        theRank = GameObject.FindObjectOfType<Rank>().GetComponent<Rank>();
        theScore =  GameObject.FindObjectOfType<Score>().GetComponent<Score>();
    }

    void Start()
    {
        if (PlayerController.instance.onTutorial)
        {
            ActivateUI(UI.HUD, false);
            ActivateUI(UI.Popup, false);
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
}
