using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Setting : MonoBehaviour
{
    enum Version
    {
        Title = 0,
        InGame
    }

    enum Tap
    {
        Sound = 0,
        Control
    }

    [SerializeField] Menu menu;
    [SerializeField] Option option;

    [Space]
    [SerializeField] GameObject sound;
    [SerializeField] GameObject control;
    [SerializeField] Image soundTapButtonImage;
    [SerializeField] Image controlTapButtonImage;
    [SerializeField] Image panel;
    [SerializeField] Color fadeColor;
    [SerializeField] Color selectedColor;
    [SerializeField] float duration;

    Version version = Version.Title;
    Tap tap = Tap.Sound;

    bool onSetting;  

    AudioClip popupClip;
    AudioClip tapClip;

    void Awake()
    {
        popupClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Popup");
        tapClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Tap");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (onSetting)
            {
                Exit();
            }
        }
    }

    #region Popup
    public void PopupSetting(bool state, int targetScale)
    {
        if (!onSetting)
        {
            onSetting = true;

            SFXController.instance.PlaySFX(popupClip, 0, 1.2f, 0.8f);

            FadeInPanel(state);
            SetScaleBox(targetScale);
            ShowSelected(tap);
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
        transform.DOScale(targetScale, duration).SetEase(Ease.OutCubic);
    }
    #endregion

    #region Tap
    public void ConvertTap(string tap)
    {
        switch (tap)
        {
            case "sound":
                this.tap = Tap.Sound;
                ShowSelected(this.tap);
                sound.SetActive(true);
                control.SetActive(false);
                SFXController.instance.PlaySFX(tapClip);
                break;
            case "control":
                this.tap = Tap.Control;
                ShowSelected(this.tap);
                sound.SetActive(false);
                control.SetActive(true);
                SFXController.instance.PlaySFX(tapClip);
                break;
        }
    }

    void ShowSelected(Tap tap)
    {
        switch (tap)
        {
            case Tap.Sound:
                soundTapButtonImage.color = selectedColor;
                controlTapButtonImage.color = Color.white;
                break;
            case Tap.Control:
                soundTapButtonImage.color = Color.white;
                controlTapButtonImage.color = selectedColor;
                break;
        }
    }
    #endregion

    #region Button Function
    public void Apply()
    {
        switch (version)
        {
            case Version.Title:
                Exit();
                //TO-DO
                break;
            case Version.InGame:
                UIManager.instance.onSetting = false;
                gameObject.SetActive(false);
                option.gameObject.SetActive(true);
                break;
        }
    }

    public void Cancle()
    {
        switch (version)
        {
            case Version.Title:
                Exit();
                break;
            case Version.InGame:
                UIManager.instance.onSetting = false;
                gameObject.SetActive(false);
                option.gameObject.SetActive(true);
                break;
        }
    }

    void Exit()
    {
        switch (version)
        {
            case Version.Title:
                menu.onLock = false;
                break;
            case Version.InGame:
                break;
            default:
                break;
        }

        onSetting = false;

        SFXController.instance.PlaySFX(popupClip, 0, 0.8f, 0.8f);

        FadeInPanel(false);
        SetScaleBox(0);
    }
    #endregion
}
