using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Setting : MonoBehaviour
{
    enum State
    {
        Title = 0,
        InGame
    }

    [SerializeField] State state = State.Title;
    [SerializeField] Menu menu;
    [SerializeField] GameObject sound;
    [SerializeField] GameObject control;
    [SerializeField] Image panel;
    [SerializeField] Color fadeColor;
    [SerializeField] float duration;

    public bool onSetting;  

    AudioClip popupClip;

    void Awake()
    {
        popupClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Popup");
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

    public void PopupSetting(bool state, int targetScale)
    {
        if (!onSetting)
        {
            onSetting = true;

            AudioManager.instance.PlaySFX(popupClip, 0, 1.2f, 0.8f);

            FadeInPanel(state);
            SetScaleBox(targetScale);
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
        transform.DOScale(targetScale, duration).SetEase(Ease.OutSine);
    }

    public void ConvertTap(string tap)
    {
        switch (tap)
        {
            case "sound":
                sound.SetActive(true);
                control.SetActive(false);
                break;
            case "control":
                sound.SetActive(false);
                control.SetActive(true);
                break;
        }
    }

    public void Apply()
    {
        switch (state)
        {
            case State.Title:
                Exit();
                //TO-DO
                break;
            case State.InGame:
                gameObject.SetActive(false);
                //UIManager.instance.Activate_Popup_Children(Children_Popup.Option);
                break;
        }
    }

    public void Cancle()
    {
        switch (state)
        {
            case State.Title:
                Exit();
                break;
            case State.InGame:
                gameObject.SetActive(false);
                //UIManager.instance.Activate_Popup_Children(Children_Popup.Option);
                break;
        }
    }

    void Exit()
    {
        switch (state)
        {
            case State.Title:
                menu.onLock = false;
                break;
            case State.InGame:
                break;
            default:
                break;
        }

        onSetting = false;

        AudioManager.instance.PlaySFX(popupClip, 0, 0.8f, 0.8f);

        FadeInPanel(false);
        SetScaleBox(0);
    }
}
