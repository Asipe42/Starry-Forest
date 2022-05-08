using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using DG.Tweening;

public class Setting : MonoBehaviour
{
    enum Tap
    {
        Sound = 0,
        Control
    }

    public event Action<bool> onSettingEvent;

    [SerializeField] Option option;

    [Header("Taps")]
    [SerializeField] GameObject sound;
    [SerializeField] GameObject control;

    [Header("UI")]
    [SerializeField] Transform box;
    [SerializeField] Image soundTapButtonImage;
    [SerializeField] Image controlTapButtonImage;
    [SerializeField] Image panel;
    [SerializeField] Color fadeColor;
    [SerializeField] Color selectedColor;
    [SerializeField] float duration;

    [Header("Sound")]
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SFXSlider;

    public const string MIXER_BGM = "BGMVolume";
    public const string MIXER_SFX = "SFXVolume";

    SceneType sceneType = SceneType.Title;
    Tap tap = Tap.Sound;

    bool onSetting;  

    AudioClip popupClip;
    AudioClip tapClip;

    void Awake()
    {
        popupClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Popup");
        tapClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Tap");
    }

    void Start()
    {
        BGMSlider.onValueChanged.AddListener(SetBGMVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);
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
            panel.DOColor(Color.clear, duration).SetEase(Ease.Linear);
    }

    void SetScaleBox(int targetScale)
    {
        box.transform.DOScale(targetScale, duration).SetEase(Ease.OutCubic);
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
        switch (sceneType)
        {
            case SceneType.Title:
                Exit();
                //TO-DO
                break;
            case SceneType.InGame:
                UIManager.instance.onSetting = false;
                gameObject.SetActive(false);
                option.gameObject.SetActive(true);
                break;
        }
    }

    public void Cancle()
    {
        switch (sceneType)
        {
            case SceneType.Title:
                Exit();
                break;
            case SceneType.InGame:
                UIManager.instance.onSetting = false;
                gameObject.SetActive(false);
                option.gameObject.SetActive(true);
                break;
        }
    }

    void Exit()
    {
        switch (sceneType)
        {
            case SceneType.Title:
                onSettingEvent.Invoke(false);
                break;
            case SceneType.InGame:
                break;
            default:
                break;
        }

        onSetting = false;

        SFXController.instance.PlaySFX(popupClip, 0, 0.8f, 0.8f);

        this.tap = Tap.Sound;
        ShowSelected(this.tap);
        sound.SetActive(true);
        control.SetActive(false);

        FadeInPanel(false);
        SetScaleBox(0);
    }
    #endregion

    #region Volume Setting
    void SetBGMVolume(float value)
    {
        mixer.SetFloat(MIXER_BGM, Mathf.Log10(value) * 20);
    }

    void SetSFXVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }
    #endregion
}
