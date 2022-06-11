using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Result : MonoBehaviour
{
    [SerializeField] RectTransform timeCard;
    [SerializeField] RectTransform itemCard;
    [SerializeField] RectTransform heartCard;
    [SerializeField] RectTransform rankBar;
    [SerializeField] Image rankBarGauge;
    [SerializeField] Sprite cardFront;

    [Header("Time")]
    [SerializeField] TextMeshProUGUI timePercentage;
    [SerializeField] Image timeGague;
    [SerializeField] TextMeshProUGUI timeScore;

    [Header("Item")]
    [SerializeField] TextMeshProUGUI itemPercentage;
    [SerializeField] Image itemGague;
    [SerializeField] TextMeshProUGUI itemScore;

    [Header("Heart")]
    [SerializeField] TextMeshProUGUI heartPercentage;
    [SerializeField] Image heartGauge;
    [SerializeField] TextMeshProUGUI heartScore;

    int timePoint;
    int itemBounusPoint;
    int heartBounusPoint;
    float elapsedTime;
    float remainingTime;
    Grade currentStageGrade;

    public bool endDirecting { get; private set; }

    AudioClip starClip;
    AudioClip resultClip;
    AudioClip rotationClip;

    void Awake()
    {
        GetAudioClip();
    }

    #region Initial Setting
    void GetAudioClip()
    {
        starClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Star");
        resultClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Result");
        rotationClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Rotation");
    }
    #endregion

    #region Result Directing
    public void PlayResultDirecting()
    {
        var appearSequence = DOTween.Sequence();

        float intervalTime = 0.3f;
        float duration = 1.5f;
        float[] cardPositionY = { 360, 960, 1560 };

        appearSequence.Append(timeCard.DOAnchorPosX(cardPositionY[0], duration).SetEase(Ease.OutQuint)).OnStart(() => SFXController.instance.PlaySFX(resultClip))
                .Insert(intervalTime, itemCard.DOAnchorPosX(cardPositionY[1], duration).SetEase(Ease.OutQuint))
                .Insert(intervalTime * 2, heartCard.DOAnchorPosX(cardPositionY[2], duration).SetEase(Ease.OutQuint))
                .OnComplete(() => PlayRotateSequence());
    }

    void PlayRotateSequence()
    {
        var rotateSequence = DOTween.Sequence();

        float duration = 0.5f;

        rotateSequence.Append(timeCard.DORotate(new Vector3(0f, 180f, 0f), duration)
                                      .SetEase(Ease.InSine)
                                      .OnPlay(() => SFXController.instance.PlaySFX(rotationClip))
                                      .OnUpdate(() => ShowCardInfo(timeCard)))
                      .AppendCallback(() => PlayTimeCardAnimation())
                      .Insert(3f, itemCard.DORotate(new Vector3(0f, 180f, 0f), duration)
                                          .OnPlay(() => SFXController.instance.PlaySFX(rotationClip))
                                          .SetEase(Ease.InSine)
                                          .OnUpdate(() => ShowCardInfo(itemCard)))
                      .AppendCallback(() => PlayItemCardAnimation())
                      .Insert(6f, heartCard.DORotate(new Vector3(0f, 180f, 0f), duration)
                                           .OnPlay(() => SFXController.instance.PlaySFX(rotationClip))
                                           .SetEase(Ease.InSine)
                                           .OnUpdate(() => ShowCardInfo(heartCard)))
                      .AppendCallback(() => PlayHeartCardAnimation());

        rotateSequence.OnComplete(() => PlayRankBarSequence());
    }

    void ShowCardInfo(RectTransform card)
    {
        if (card.eulerAngles.y > 90f)
        {
            card.GetComponent<Image>().sprite = cardFront;

            for (int i = 0; i < card.childCount; i++)
            {
                card.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    void PlayTimeCardAnimation()
    {
        GetTimeInfo();
        float target = 0f;

        string gradeCharacter = "";

        switch (currentStageGrade)
        {
            case Grade.APlus:
                gradeCharacter = "A+";
                target = 0.16f;
                break;
            case Grade.A:
                gradeCharacter = "A";
                target = 0.33f;
                break;
            case Grade.BPlus:
                gradeCharacter = "B+";
                target = 0.5f;
                break;
            case Grade.B:
                gradeCharacter = "B";
                target = 0.66f;
                break;
            case Grade.CPlus:
                gradeCharacter = "C+";
                target = 0.83f;
                break;
            case Grade.C:
                gradeCharacter = "C";
                target = 1f;
                break;
        }

        timePercentage.text = string.Format("{0:0}ÃÊ  {1:0}", elapsedTime, gradeCharacter);
        timeScore.text = "+" + timePoint + " Á¡";

        var sequence = DOTween.Sequence();

        sequence.Append(timePercentage.DOScale(1f, 0.5f)
                                      .SetEase(Ease.OutBounce)
                                      .OnPlay(() => SFXController.instance.PlaySFX(starClip))
                                      .OnComplete(() => timeGague.DOFillAmount(target, 1f)))
                .Append(timeScore.DOScale(1f, 0.5f)
                                 .SetEase(Ease.OutBounce)
                                 .OnPlay(() => SFXController.instance.PlaySFX(starClip)));
    }

    void PlayItemCardAnimation()
    {
        float rate = CalculateItemPoint();
        float target = 0f;

        if (rate >= 75)
        {
            target = 1f;
            itemBounusPoint = 70;
        }
        else if (rate >= 50)
        {
            target = 0.66f;
            itemBounusPoint = 50;
        }
        else if (rate >= 25)
        {
            target = 0.33f;
            itemBounusPoint = 30;
        }
        else
        {
            itemGague.DOFillAmount(0f, 1f);
            itemBounusPoint = 0;
        }

        itemPercentage.text = string.Format("{0:0}%", rate);
        itemScore.text = "+" + itemBounusPoint + " Á¡";

        var sequence = DOTween.Sequence();

        sequence.Append(itemPercentage.DOScale(1f, 0.5f)
                                      .SetEase(Ease.OutBounce)
                                      .OnPlay(() => SFXController.instance.PlaySFX(starClip))
                                      .OnComplete(() => itemGague.DOFillAmount(target, 1f)))
                .Append(itemScore.DOScale(1f, 0.5f)
                                 .SetEase(Ease.OutBounce)
                                 .OnPlay(() => SFXController.instance.PlaySFX(starClip)));     
    }

    void PlayHeartCardAnimation()
    {
        float rate = CalculateHeartPoint();
        float target = 0f;
        float heart = PlayerController.instance.theStatus.currentHp;

        if (rate >= 90)
        {
            target = 1f;
            heartPercentage.text = "3Ä­";
            heartBounusPoint = 100;
        }
        else if (rate >= 60)
        {
            target = 0.66f;
            heartPercentage.text = "2Ä­";
            heartBounusPoint = 50;
        }
        else
        {
            target = 0.33f;
            heartPercentage.text = "1Ä­";
            heartBounusPoint = 30;
        }

        heartPercentage.text = string.Format("{0} Ä­", heart);
        heartPercentage.text = "+" + itemBounusPoint + " Á¡";

        var sequence = DOTween.Sequence();

        sequence.Append(heartPercentage.DOScale(1f, 0.5f)
                                       .SetEase(Ease.OutBounce)
                                       .OnPlay(() => SFXController.instance.PlaySFX(starClip))
                                       .OnComplete(() => heartGauge.DOFillAmount(target, 1f)))
                .Append(heartScore.DOScale(1f, 0.5f)
                                  .SetEase(Ease.OutBounce)
                                  .OnPlay(() => SFXController.instance.PlaySFX(starClip)));
    }

    void PlayRankBarSequence()
    {
        float gaugeRate = (float)(itemBounusPoint + heartBounusPoint) / 100f;

        var sequence = DOTween.Sequence();

        sequence.Append(rankBar.DOAnchorPosY(50f, 0.5f).OnComplete(() => rankBarGauge.DOFillAmount(gaugeRate, 2f).SetEase(Ease.OutSine)))
                .InsertCallback(5f, () => endDirecting = true);
          
        if (itemBounusPoint + heartBounusPoint > 100)
        {
            if (currentStageGrade > Grade.APlus)
            {
                currentStageGrade--;
            }
        }
    }
    #endregion

    #region Calculate
    float CalculateItemPoint()
    {
        int totalItemCount = FloorManager.instance.totalItemCount;
        int takedItemCount = UIManager.instance.score.totalScore;

        float rate = (float)takedItemCount / (float)totalItemCount * 100f;
        return rate;
    }

    float CalculateHeartPoint()
    {
        int maxHp = PlayerController.instance.theStatus.maxHp;
        int currentHp = PlayerController.instance.theStatus.currentHp;

        float rate = (float)currentHp / (float)maxHp * 100;
        return rate;
    }

    void GetTimeInfo()
    {
        this.elapsedTime = UIManager.instance.rank.ElapsedTime;
        remainingTime = UIManager.instance.rank.CurrentTime;
        currentStageGrade = UIManager.instance.rank.grade;

        switch (currentStageGrade)
        {
            case Grade.APlus:
                timePoint = 500;
                break;
            case Grade.A:
                timePoint = 400;
                break;
            case Grade.BPlus:
                timePoint = 300;
                break;
            case Grade.B:
                timePoint = 200;
                break;
            case Grade.CPlus:
                timePoint = 100;
                break;
            case Grade.C:
                timePoint = 0;
                break;
        }
    }

    Grade GetRank()
    {
        return currentStageGrade;
    }
    #endregion
}
