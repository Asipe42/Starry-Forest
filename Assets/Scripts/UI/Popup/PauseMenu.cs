using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Image panel;
    [SerializeField] RectTransform box;

    AudioClip popupClip;

    void Awake()
    {
        GetAudioClip();
    }

    void GetAudioClip()
    {
        popupClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Popup");
    }

    public void SetActivation(bool state)
    {
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
    public void Restart()
    {
        Loading.LoadScene(StageManager.instance.stageTemplate.currentSceneName);
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
