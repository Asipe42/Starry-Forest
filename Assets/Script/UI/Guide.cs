using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Guide : MonoBehaviour
{
    MenuType menuType = MenuType.NewGame;

    Menu menu;

    [Header("UI")]
    [SerializeField] Transform box;
    [SerializeField] Image panel;
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] TextMeshProUGUI subText;

    [Header("Values")]
    [SerializeField] Vector3 textPosition_NewGame;
    [SerializeField] Vector3 textPosition_Exit;
    [SerializeField] Color fadeColor;
    [SerializeField] float duration;
    [SerializeField] string[] Message_NewGame;
    [SerializeField] string Message_Exit;

    [Space]
    [SerializeField] bool onGuide;

    [Space]
    [SerializeField] AudioClip popupClip;

    void Awake()
    {
        menu = FindObjectOfType<Menu>();

        popupClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Popup");
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

            AudioManager.instance.PlaySFX(popupClip, 0, 1.2f, 0.8f);

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
        box.DOScale(targetScale, duration).SetEase(Ease.OutSine);
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
            default:
                Debug.LogError("not allocated menu type");
                break;
        }
    }

    public void Accept()
    {
        switch (menuType)
        {
            case MenuType.NewGame:
                Loading.LoadScene("Stage_01_Tutorial");
                break;
            case MenuType.Exit:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
                break;
            default:
                Debug.LogError("not allocated menu type");
                break;
        }
    }

    public void Cancle()
    {
        onGuide = false;
        menu.onLock = false;

        AudioManager.instance.PlaySFX(popupClip, 0, 0.8f, 0.8f);

        FadeInPanel(false);
        SetScaleBox(0);
    }
}
