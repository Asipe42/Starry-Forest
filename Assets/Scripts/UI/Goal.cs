using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Goal : MonoBehaviour
{
    [SerializeField] GameObject box;
    [SerializeField] TextMeshProUGUI text;

    public bool endGoal;

    AudioClip goalClip;

    void Awake()
    {
        goalClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Goal");
    }

    public void ShowGoal()
    {
        box.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce)
                     .OnStart(() => SFXController.instance.PlaySFX(goalClip))
                     .OnComplete(() =>
                     {
                         box.GetComponent<Image>().DOFade(0f, 2.5f);
                         text.DOFade(0f, 2.5f)
                             .OnComplete(() => endGoal = true);
                     });            
    }
}
