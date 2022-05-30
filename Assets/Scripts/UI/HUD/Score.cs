using UnityEngine;
using DG.Tweening;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    public int totalScore { get; private set; }

    Vector3 originalScale;

    void Awake()
    {
        Initialize();
        CheckScore(0);
    }

    #region Initial Setting
    void Initialize()
    {
        originalScale = scoreText.transform.localScale;
    }
    #endregion

    /// <summary>
    /// totalScore에 score를 누계하고 ScoreUI를 조정한다.
    /// </summary>
    /// <param name="score"></param>
    public void CheckScore(int score)
    {
        this.totalScore += score;

        scoreText.text = this.totalScore.ToString();

        var sequence = DOTween.Sequence();

        sequence.Append(scoreText.transform.DOScale(new Vector3(originalScale.x + 0.2f, originalScale.y + 0.2f, originalScale.z + 0.2f), 0.05f).SetEase(Ease.Linear))
                .Append(scoreText.transform.DOScale(originalScale, 0.1f).SetEase(Ease.Linear));
    }
}
