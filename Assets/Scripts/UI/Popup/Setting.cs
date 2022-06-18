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

    enum DirectionType
    {
        Up,
        Down,
        Right,
        Left
    }

    enum ButtonType
    {
        None = 0,
        Sound,
        Control,
        BGMSlider,
        SFXSlider,
        Accpet,
        Cancle
    }

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
    ButtonType currentButtonType = ButtonType.None;

    bool onSetting;  

    AudioClip popupClip;
    AudioClip tapClip;
    AudioClip errorClip;

    public event Action<bool> onSettingEvent;

    void Awake()
    {
        Initialize();
        GetAudioClip();
        SetVolumes();
    }

    #region Initial Setting
    void Initialize()
    {
        this.tap = Tap.Sound;
        ShowSelected(this.tap);
        sound.SetActive(true);
        control.SetActive(false);
    }

    void GetAudioClip()
    {
        popupClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Popup");
        tapClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Tap");
        errorClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Error");
    }

    void SetVolumes()
    {
        BGMSlider.onValueChanged.AddListener(SetBGMVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }
    #endregion

    void Update()
    {
        InputKey();
    }

    void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (onSetting)
            {
                Exit();
            }
        }

        if (onSetting)
        {
            Cursor.visible = true;

            bool onRight = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
            bool onLeft = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
            bool onUp = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
            bool onDown = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);
            bool onSelect = Input.GetButtonDown("Submit");

            if (onRight && !onLeft)
            {

            }

            if (onLeft && !onRight)
            {

            }

            if (onUp && !onDown)
            {

            }

            if (onDown && !onUp)
            {

            }

            if (onSelect)
            {

            }
        }
    }

    #region Popup
    /// <summary>
    /// Setting UI를 활성화/비활성화 합니다.
    /// </summary>
    /// <param name="state"></param>
    public void SetActivation(bool state, float duration = 0.2f)
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
                panel.raycastTarget = true;
            }

            box.DOScale(1f, duration).SetEase(Ease.OutQuad);

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
                panel.raycastTarget = false;
            }

            box.DOScale(0f, duration).SetEase(Ease.OutQuad);
        }
    }
    #endregion

    #region Tap
    /// <summary>
    /// tap을 변경합니다.
    /// </summary>
    /// <param name="tap"></param>
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
    /// <summary>
    /// 변경 사항을 적용하고 Setting UI를 닫습니다.
    /// </summary>
    public void Apply()
    {
        Exit();
    }

    /// <summary>
    /// 변경 사항을 취소하고 Setting UI를 닫습니다.
    /// </summary>
    public void Cancle()
    {
        Exit();
    }

    public void PlayDisabledAnimation()
    {
        box.transform.DOShakePosition(0.5f, new Vector3(15f, 0f, 0f), randomness: 0f).SetEase(Ease.OutQuad);
        SFXController.instance.PlaySFX(errorClip);
    }
    #endregion

    void Exit()
    {
        if (sceneType == SceneType.Title)
            onSettingEvent.Invoke(false);

        onSetting = false;
        Cursor.visible = false;

        Initialize();

        SetActivation(false);
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
