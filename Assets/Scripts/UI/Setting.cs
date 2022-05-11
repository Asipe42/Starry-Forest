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

    [SerializeField] SceneType sceneType = SceneType.Title;

    [Header("Taps")]
    [SerializeField] GameObject sound;
    [SerializeField] GameObject control;

    [Header("UI")]
    [SerializeField] RectTransform box;
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
    public void SetActivation(bool state)
    {
        onSetting = state;

        if (state)
        {
            SFXController.instance.PlaySFX(
                    clip: popupClip,
                    delay: 0,
                    pitch: 1.25f,
                    volume: 0.75f);

            if (sceneType == SceneType.Title)
            {
                panel.DOFade(0.3f, 0.3f);
            }

            box.DOScale(1f, 0.5f).SetEase(Ease.InBounce);

            ShowSelected(tap);
        }
        else
        {
            SFXController.instance.PlaySFX(
                    clip: popupClip,
                    delay: 0,
                    pitch: 0.75f,
                    volume: 0.75f);

            if (sceneType == SceneType.Title)
            {
                panel.DOFade(0, 0.3f);
            }

            box.DOScale(0f, 0.5f).SetEase(Ease.InBounce);
        }
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
        Exit();
    }

    public void Cancle()
    {
        Exit();
    }
    #endregion

    void Exit()
    {
        if (sceneType == SceneType.Title)
            onSettingEvent.Invoke(false);

        onSetting = false;

        Initlaize();

        SetActivation(false);
    }

    void Initlaize()
    {
        this.tap = Tap.Sound;
        ShowSelected(this.tap);
        sound.SetActive(true);
        control.SetActive(false);
    }

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
