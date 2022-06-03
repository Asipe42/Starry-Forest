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
               .SetEase(Ease.OutBounce)
               .OnComplete(() => Time.timeScale = 0f );
        }
        else
        {
            InputManager.instance.onLock = false;

            Time.timeScale = 1f;

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
    /// �ش� ���������� �ٽ� �����մϴ�.
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1f;

        Loading.LoadScene(StageManager.instance.stageTemplate.currentSceneName);
    }

    /// <summary>
    /// ���� UI�� ���ϴ�.
    /// </summary>
    public void GoSetting()
    {
        UIManager.instance.ShowSetting(true);
    }

    /// <summary>
    /// Ÿ��Ʋ ������ ���ư��ϴ�.
    /// </summary>
    public void GoTitle()
    {
        Loading.LoadScene("Title");
    }

    /// <summary>
    /// ������ �簳�մϴ�.
    /// </summary>
    public void Cancle()
    {
        UIManager.instance.HideOption();
    }
}
