using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Option : MonoBehaviour
{
    [SerializeField] Image panel;
    [SerializeField] RectTransform box;
    [SerializeField] string currentSceneName;

    AudioClip popupClip;

    void Awake()
    {
        popupClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Popup");
    }

    public void SetActivation(bool state)
    {
        if (state)
        {
            SFXController.instance.PlaySFX(
                    clip: popupClip,
                    delay: 0,
                    pitch: 1.25f,
                    volume: 0.75f);

            panel.DOFade(0.3f, 0.3f);
            box.DOScale(1f, 0.5f)
                .SetEase(Ease.InBounce)
                .OnComplete(() => Time.timeScale = 0f );
        }
        else
        {
            SFXController.instance.PlaySFX(
                    clip: popupClip,
                    delay: 0,
                    pitch: 0.75f,
                    volume: 0.75f);

            panel.DOFade(0, 0.3f);
            box.DOScale(0f, 0.5f)
                .SetEase(Ease.InBounce)
                .OnComplete(() => Time.timeScale = 1f);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;

        Loading.LoadScene(currentSceneName);
    }

    public void GoSetting()
    {
        UIManager.instance.ShowSetting(true);
    }

    public void GoTitle()
    {
        Loading.LoadScene("Title");
    }

    public void Cancle()
    {
        UIManager.instance.HideOption();
    }
}
