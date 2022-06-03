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

    [Header("Values")]
    [SerializeField] Vector3 textPosition_NewGame;
    [SerializeField] Vector3 textPosition_Exit;
    [SerializeField] Color fadeColor;
    [SerializeField] float duration;
    [SerializeField] string[] Message_NewGame;
    [SerializeField] string Message_Exit;
    [SerializeField] bool onGuide;

    Menu menu;

    AudioClip popupClip;
    AudioClip acceptClip;

    bool onAccept;

    public static event Action<bool> cancleGuideEvent;

    void Awake()
    {
        Initialize();
        GetAudioClip();
    }
    
    void Initialize()
    {
        menu = GameObject.FindObjectOfType<Menu>();
    }

    void GetAudioClip()
    {
        popupClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Popup");
        acceptClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Accept");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (onGuide)
            {
                Cancle();
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
                mainText.text = Message_NewGame[0];
                subText.text = Message_NewGame[1];
                break;
            case MenuType.Exit:
                mainText.rectTransform.anchoredPosition = textPosition_Exit;
                mainText.text = Message_Exit;
                subText.text = "";
                break;
        }
    }

    public void Accept()
    {
        switch (menuType)
        {
            case MenuType.NewGame:
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

    void PlayAcceptAnimation()
    {
        SFXController.instance.PlaySFX(acceptClip);
        BGMController.instance.FadeVolume(0f, 1f, 1f);
        menu.fadeScreen.FadeScreenEffect(1f, 1f, 1f);
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
