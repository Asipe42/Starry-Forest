using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Rank : MonoBehaviour
{
    public enum Grade
    {
        APlus, A,
        BPlus, B,
        CPlus, C
    }

    public Grade grade = Grade.APlus;

    [Header("Image/Text")]
    [SerializeField] Image rankEmpty;
    [SerializeField] Image rankFull;
    [SerializeField] TextMeshProUGUI timeText;

    [Header("Rank")]
    [SerializeField] Sprite[] rankSpriteFull;
    [SerializeField] Sprite[] rankSpriteEmtpy;

    [Space]
    [SerializeField] int gradeSize = 6;
    [SerializeField] float[] timeLimit;
    [SerializeField] bool[] onRank;

    AudioClip changeClip;

    Vector3 originalScale_rankFull;
    Vector3 originalScale_rankEmpty;

    float time = 0f;

    void Awake()
    {
        changeClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Change");
    }

    void Start()
    {
        originalScale_rankFull = rankFull.transform.localScale;
        originalScale_rankEmpty = rankEmpty.transform.localScale;
    }

    void Update()
    {
        DisplayTime();
        CalculateGrade();
        CheckTime();
    }

    void DisplayTime()
    {
        time += Time.deltaTime;
        timeText.text = string.Format("{0:0.0}", time);
    }

    void CalculateGrade()
    {
        if (grade == Grade.C)
        {
            rankFull.fillAmount = 1;
            return;
        }

        if (grade < Grade.C)
        {
            if (grade == Grade.APlus)
                rankFull.fillAmount = 1 - (time / timeLimit[(int)grade]);
            else
                rankFull.fillAmount = 1 - ((time - timeLimit[(int)grade - 1]) / (timeLimit[(int)grade] - timeLimit[(int)grade - 1]));
        }
    }

    void CheckTime()
    {
        for (int i = 0; i < gradeSize; i++)
        {
            if (onRank[i])
                continue;

            if (this.time > timeLimit[i])
            {
                if (i < gradeSize && grade < Grade.C)
                {
                    onRank[i] = true;
                    DowngradeRank();
                }
            }
        }
    }

    void DowngradeRank()
    {
        grade++;
        ChangeRankImage(grade);
    }

    void ChangeRankImage(Grade grade)
    {      
        DOTween.Sequence()
            .Append(rankFull.transform.DOScale(new Vector3(originalScale_rankFull.x + 0.5f, originalScale_rankFull.y + 0.5f, originalScale_rankFull.z + 0.5f), 0.2f)
            .SetEase(Ease.Linear))
            .Append(rankFull.transform.DOScale(originalScale_rankFull, 0.2f)
            .SetEase(Ease.Linear));

        DOTween.Sequence()
            .Append(rankEmpty.transform.DOScale(new Vector3(originalScale_rankEmpty.x + 0.5f, originalScale_rankEmpty.y + 0.5f, originalScale_rankEmpty.z + 0.5f), 0.2f)
            .SetEase(Ease.Linear))
            .Append(rankEmpty.transform.DOScale(originalScale_rankEmpty, 0.2f)
            .SetEase(Ease.Linear));

        SFXController.instance.PlaySFX(changeClip);

        rankFull.sprite = rankSpriteFull[(int)grade];
        rankEmpty.sprite = rankSpriteEmtpy[(int)grade];
    }
}
