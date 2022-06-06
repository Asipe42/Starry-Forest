using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ResultSign : MonoBehaviour
{
    [SerializeField] GameObject box;
    [SerializeField] TextMeshProUGUI text;

    public bool endDirecting;

    AudioClip goalClip;

    void Awake()
    {
        GetAudioClip();
    }

    void GetAudioClip()
    {
        goalClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Goal");
    }

    public void ShowResultSign(string message)
    {
        BGMController.instance.FadeVolume(0f, 2.5f);

        text.text = message;

        var sequence = DOTween.Sequence();

        sequence.Append(box.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce).OnStart(() => SFXController.instance.PlaySFX(goalClip)))
                .Insert(1.5f, box.GetComponent<Image>().DOFade(0f, 2f))
                .Insert(1.5f, text.DOFade(0f, 2f))
                .AppendCallback(() => endDirecting = true);
    }
}