using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public enum Grade
{
    None = 0,
    APlus, A,
    BPlus, B,
    CPlus, C
}

public class Rank : MonoBehaviour
{
    enum TimeState
    {
        None = 0,
        Default,
        Warning
    }

    [Header("UI")]
    [SerializeField] Image rankEmpty;
    [SerializeField] Image rankFull;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Sprite[] rankSpriteFull;
    [SerializeField] Sprite[] rankSpriteEmtpy;

    [Header("Values")]
    [SerializeField] Color warningTextColor;
    [SerializeField] float maxTime = 300f;
    [SerializeField] float[] timeLimit;
    [SerializeField] bool[] onRank;

    float currentTime;

    bool onGameOver;

    public Grade grade = Grade.APlus;
    TimeState timeState = TimeState.Default;

    public float ElapsedTime
    {
        get
        {
            return maxTime - currentTime;
        }
    }

    public float CurrentTime
    {
        get { return currentTime; }
        set { currentTime = value; }
    }

    AudioClip changeClip;
    AudioClip warningClip;

    Vector3 originalScale_rankFull;
    Vector3 originalScale_rankEmpty;


    void Awake()
    {
        GetAudioClip();
        Initialize();
    }

    #region Initial Setting
    void GetAudioClip()
    {
        changeClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Change");
    }

    void Initialize()
    {
        currentTime = maxTime;
        originalScale_rankFull = rankFull.transform.localScale;
        originalScale_rankEmpty = rankEmpty.transform.localScale;
    }
    #endregion

    void Update()
    {
        if (!PlayerController.instance.onGoal && !onGameOver)
        {
            CalculateGrade();
            DisplayTime();
            CheckTime();
            CheckGameOver();
            MonitorGrade();
        }
    }

    void CalculateGrade()
    {
        if (grade < Grade.C)
        {
            rankFull.fillAmount = (currentTime - timeLimit[(int)grade]) / (timeLimit[(int)grade - 1] - timeLimit[(int)grade]);
        }
        else
        {
            rankFull.fillAmount = currentTime / timeLimit[(int)grade - 1];
        }
    }

    void DisplayTime()
    {
        currentTime -= Time.deltaTime;
        timeText.text = string.Format("{0:0.0}", currentTime);
    }

    void CheckTime()
    {
        for (int i = 0; i < onRank.Length; i++)
        {
            if (onRank[i])
                continue;

            if (this.currentTime < timeLimit[i])
            {
                onRank[i] = true;
                DowngradeRank();
            }
        }
    }

    void CheckGameOver()
    {
        if (currentTime < 0)
        {
            onGameOver = true;

            currentTime = 0;
            StartCoroutine(PlayerController.instance.Dead());
        }
    }

    void DowngradeRank()
    {
        grade++;

        ChangeRankImage(grade);
    }

    void ChangeRankImage(Grade grade)
    {
        SFXController.instance.PlaySFX(changeClip);

        DOTween.Sequence()
               .Append(rankFull.transform.DOScale(new Vector3(originalScale_rankFull.x + 0.5f, originalScale_rankFull.y + 0.5f, originalScale_rankFull.z + 0.5f), 0.2f).SetEase(Ease.Linear))
               .Append(rankFull.transform.DOScale(originalScale_rankFull, 0.2f).SetEase(Ease.Linear));

        DOTween.Sequence()
               .Append(rankEmpty.transform.DOScale(new Vector3(originalScale_rankEmpty.x + 0.5f, originalScale_rankEmpty.y + 0.5f, originalScale_rankEmpty.z + 0.5f), 0.2f).SetEase(Ease.Linear))
               .Append(rankEmpty.transform.DOScale(originalScale_rankEmpty, 0.2f).SetEase(Ease.Linear));

        rankFull.sprite = rankSpriteFull[(int)grade - 1];
        rankEmpty.sprite = rankSpriteEmtpy[(int)grade - 1];
    }

    void MonitorGrade()
    {
        if (grade >= Grade.CPlus)
        {
            if (timeState != TimeState.Warning)
            {
                timeState = TimeState.Warning;
                PlayWarningClip(timeState);
            }
        }
        else
        {
            if (timeState != TimeState.Default)
            {
                timeState = TimeState.Default;
            }
        }

        ChangeTimeTextColor(timeState);
    }

    void ChangeTimeTextColor(TimeState timeState)
    {
        if (timeState == TimeState.Warning)
        {
            timeText.color = warningTextColor;
        }
        else
        {
            timeText.color = Color.white;
        }
    }

    void PlayWarningClip(TimeState timeState)
    {
        SFXController.instance.PlaySFX(warningClip);
    }
}