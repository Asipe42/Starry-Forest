using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Guide : MonoBehaviour
{
    MenuType menuType = MenuType.NewGame;

    [Header("UI")]
    [SerializeField] Transform box;
    [SerializeField] Image panel;
    [SerializeField] Image acceptButtonImage;
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] TextMeshProUGUI subText;
    [SerializeField] TextMeshProUGUI accpetText;
    [SerializeField] TextMeshProUGUI cancleText;

    [Header("Values")]
    [SerializeField] Vector3 textPosition_NewGame;
    [SerializeField] Vector3 textPosition_Exit;
    [SerializeField] Color fadeColor;
    [SerializeField] float duration;
    [SerializeField] string[] Message_newGame;
    [SerializeField] string[] Message_newGameButton;
    [SerializeField] string Message_exit;
    [SerializeField] string[] Message_exitButton;
    [SerializeField] string Message_goTitle;
    [SerializeField] string[] Message_goTitleButton;
    [SerializeField] bool onGuide;
    [SerializeField] bool isMap;
    [SerializeField] FadeScreen fadeScreen;

    //Menu menu;

    AudioClip popupClip;
    AudioClip acceptClip;

    bool onAccept;

    public static event Action<bool> cancleGuideEvent;

    void Awake()
    {
        GetAudioClip();
    }

    void GetAudioClip()
    {
        popupClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Popup");
        acceptClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Appear");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMap)
            {
                if (!onGuide)
                {
                    PopupGuide_Map(onGuide, 1);
                }
                else
                {
                    Cancle();
                }
            }
            else
            {
                if (onGuide)
                {
                    Cancle();
                }
            }
        }
    }

    public void PopupGuide(bool state, int targetScale, MenuType menuType)
    {
        if (!onGuide)
        {
            this.menuType = menuType;
            onGuide = true;

            SFXController.instance.PlaySFX(popupClip, 0f, 1.25f, 0.3f);

            FadeInPanel(state);
            SetScaleBox(targetScale);
            SetText();
        }
    }

    public void PopupGuide_Map(bool state, int targetScale)
    {
        onGuide = true;

        SFXController.instance.PlaySFX(popupClip, 0f, 1.25f, 0.3f);

        FadeInPanel(state);
        SetScaleBox(targetScale);
        SetText_Map();
    }

    void FadeInPanel(bool state)
    {
        if (state)
            panel.DOColor(fadeColor, duration).SetEase(Ease.Linear);
        else
            panel.DOColor(new Color(0f, 0f, 0f, 0f), duration).SetEase(Ease.Linear);
    }

    void SetScaleBox(int targetScale)
    {
        if (targetScale > 0) // appear
        {
            box.DOScale(targetScale, duration).SetEase(Ease.OutCubic);
        }
        else // disappear
        {
            box.DOScale(targetScale, duration).SetEase(Ease.OutCubic);
        }
    }

    public void SetText()
    {
        switch (menuType)
        {
            case MenuType.NewGame:
                mainText.rectTransform.anchoredPosition = textPosition_NewGame;
                SetText(Message_newGame[0], Message_newGame[1], Message_newGameButton);
                break;
            case MenuType.Exit:
                mainText.rectTransform.anchoredPosition = textPosition_Exit;
                SetText(Message_exit, "", Message_exitButton);
                break;
        }
    }

    public void SetText_Map()
    {
        mainText.rectTransform.anchoredPosition = textPosition_Exit;
        SetText(Message_goTitle, "", Message_goTitleButton);
    }

    void SetText(string mainText, string subText, string[] buttonText)
    {
        this.mainText.text = mainText;
        this.subText.text = subText;
        accpetText.text = buttonText[0];
        cancleText.text = buttonText[1];
    }

    public void Accept()
    {
        switch (menuType)
        {
            case MenuType.NewGame:
                InitializeStageInfo();
                StartCoroutine(AcceptLogic(3f));
                break;
            case MenuType.Exit:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
                break;
        }
    }

    void InitializeStageInfo()
    {
        GameManager.InitializeStageButtonInfo();
    }

    public void Accept_Map()
    {
        StartCoroutine(AcceptLogic_Map(3f));
    }

    IEnumerator AcceptLogic(float delay)
    {
        if (!onAccept)
        {
            onAccept = true;

            PlayAcceptAnimation();

            yield return new WaitForSeconds(delay);
            Loading.LoadScene("Map");
        }
    }

    IEnumerator AcceptLogic_Map(float delay)
    {
        if (!onAccept)
        {
            onAccept = true;

            PlayAcceptAnimation();

            yield return new WaitForSeconds(delay);
            Loading.LoadScene("Title");
        }
    }

    void PlayAcceptAnimation()
    {
        SFXController.instance.PlaySFX(acceptClip);
        BGMController.instance.FadeVolume(0f, 1f, 1f);
        fadeScreen.FadeScreenEffect(1f, 1f, 1f);
        acceptButtonImage.transform.DOPunchRotation(new Vector3(0f, 0f, 7.5f), 0.5f, 10, 0.5f).SetEase(Ease.OutQuad);
    }

    public void Cancle()
    {
        if (onAccept)
            return;

        onGuide = false;
        cancleGuideEvent.Invoke(false);

        SFXController.instance.PlaySFX(popupClip, 0f, 0.75f, 0.3f);

        FadeInPanel(false);
        SetScaleBox(0);
    }
}
