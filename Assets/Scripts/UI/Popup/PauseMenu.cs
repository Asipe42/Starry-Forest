using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class PauseMenu : MonoBehaviour
{
    enum DirectionType
    {
        Up,
        Down
    }

    enum ButtonType
    {
        None = 0,
        GoMap,
        GoSetting,
        GoTitle,
        Cancle
    }

    [SerializeField] Image panel;
    [SerializeField] RectTransform box;
    [SerializeField] Image[] buttons;

    AudioClip popupClip;
    AudioClip menuClip;
    AudioClip errorClip;

    bool onPauseMenu;

    ButtonType currentButtonType;

    void Awake()
    {
        GetAudioClip();
    }

    #region Initiali Setting
    void GetAudioClip()
    {
        popupClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Popup");
        errorClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Error");
        menuClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Menu");
    }
    #endregion

    void Update()
    {
        InputKey();
    }

    void InputKey()
    {
        if (onPauseMenu)
        {
            bool onUp = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
            bool onDown = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);
            bool onSelect = Input.GetButtonDown("Submit");

            if (onUp && !onDown)
            {
                ChangeSelectedButton(DirectionType.Up);
            }
            else if (onDown && !onUp)
            {
                ChangeSelectedButton(DirectionType.Down);
            }

            if (onSelect)
            {
                Select(currentButtonType);
            }
        }
    }

    void ChangeSelectedButton(DirectionType directionType)
    {
        if (directionType == DirectionType.Up)
        {
            switch (currentButtonType)
            {
                case ButtonType.None:
                    currentButtonType = ButtonType.Cancle;
                    break;
                case ButtonType.GoMap:
                    currentButtonType = ButtonType.Cancle;
                    break;
                case ButtonType.GoSetting:
                    currentButtonType = ButtonType.GoMap;
                    break;
                case ButtonType.GoTitle:
                    currentButtonType = ButtonType.GoSetting;
                    break;
                case ButtonType.Cancle:
                    currentButtonType = ButtonType.GoTitle;
                    break;
            }
        }

        if (directionType == DirectionType.Down)
        {
            switch (currentButtonType)
            {
                case ButtonType.None:
                    currentButtonType = ButtonType.GoMap;
                    break;
                case ButtonType.GoMap:
                    currentButtonType = ButtonType.GoSetting;
                    break;
                case ButtonType.GoSetting:
                    currentButtonType = ButtonType.GoTitle;
                    break;
                case ButtonType.GoTitle:
                    currentButtonType = ButtonType.Cancle;
                    break;
                case ButtonType.Cancle:
                    currentButtonType = ButtonType.GoMap;
                    break;
            }
        }

        UpdateButtonType(currentButtonType);
    }

    void UpdateButtonType(ButtonType buttonType)
    {
        SFXController.instance.PlaySFX(menuClip);

        foreach (var button in buttons)
        {
            button.color = Color.gray;
        }

        buttons[(int)buttonType - 1].color = Color.white;
    }

    void Select(ButtonType buttonType)
    {
        switch (buttonType)
        {
            case ButtonType.GoMap:
                GoMap();
                break;
            case ButtonType.GoSetting:
                GoSetting();
                break;
            case ButtonType.GoTitle:
                GoTitle();
                break;
            case ButtonType.Cancle:
                Cancle();
                break;
        }
    }

    public void SetActivation(bool state)
    {
        onPauseMenu = state;

        if (state)
        {

            InputManager.instance.onLock = true;

            SFXController.instance.PlaySFX(
                    clip: popupClip,
                    delay: 0,
                    pitch: 1.25f,
                    volume: 0.75f);

            panel.DOFade(0.3f, 0.3f);
            box.DOScale(1f, 0.5f)
               .SetEase(Ease.OutBounce);
        }
        else
        {
            InputManager.instance.onLock = false;

            SFXController.instance.PlaySFX(
                    clip: popupClip,
                    delay: 0,
                    pitch: 0.75f,
                    volume: 0.75f);

            panel.DOFade(0, 0.3f);
            box.DOScale(0f, 0.25f)
               .SetEase(Ease.OutQuad);
        }
    }
    /// <summary>
    /// 해당 스테이지를 다시 시작합니다.
    /// </summary>
    public void GoMap()
    {
        Loading.LoadScene("Map");
    }

    public void PlayDisabledAnimation()
    {
        box.transform.DOShakePosition(0.5f, new Vector3(15f, 0f, 0f), randomness: 0f).SetEase(Ease.OutQuad);
        SFXController.instance.PlaySFX(errorClip);
    }

    /// <summary>
    /// 세팅 UI를 엽니다.
    /// </summary>
    public void GoSetting()
    {
        UIManager.instance.ShowSetting(true);
    }

    /// <summary>
    /// 타이틀 씬으로 돌아갑니다.
    /// </summary>
    public void GoTitle()
    {
        Loading.LoadScene("Title");
    }

    /// <summary>
    /// 게임을 재개합니다.
    /// </summary>
    public void Cancle()
    {
        UIManager.instance.HideOption();
    }
}
