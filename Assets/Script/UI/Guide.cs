using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Guide : MonoBehaviour
{
    MenuType menuType = MenuType.NewGame;

    [SerializeField] Transform box;
    [SerializeField] Image panel;
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] TextMeshProUGUI subText;

    [SerializeField] Vector3 textPosition_NewGame;
    [SerializeField] Vector3 textPosition_Exit;
    [SerializeField] Color fadeColor;
    [SerializeField] float duration;
    [SerializeField] string[] Message_NewGame;
    [SerializeField] string Message_Exit;

    public void PopupGuide(bool state, int targetScale, MenuType menuType)
    {
        this.menuType = menuType;

        FadeInPanel(state);
        SetScaleBox(targetScale);
        SetText();
    }

    public void FadeInPanel(bool state)
    {
        if (state)
            panel.DOColor(fadeColor, duration).SetEase(Ease.Linear);
        else
            panel.DOColor(new Color(0f, 0f, 0f, 0f), duration).SetEase(Ease.Linear);
    }

    public void SetScaleBox(int targetScale)
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
                Loading.LoadScene("Stage_01");
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
        FadeInPanel(false);
        SetScaleBox(0);
    }
}
