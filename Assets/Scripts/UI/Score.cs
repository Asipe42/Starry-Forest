using UnityEngine;
using DG.Tweening;
using TMPro;

public class Score : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("Score")]
    public int score;

    Vector3 originalScale;

    void Start()
    {
        originalScale = scoreText.transform.localScale;
        CheckScore(0);
    }

    public void CheckScore(int score)
    {
        this.score += score;
        scoreText.text = this.score.ToString();

        DOTween.Sequence()
            .Append(scoreText.transform.DOScale(new Vector3(originalScale.x + 0.2f, originalScale.y + 0.2f, originalScale.z + 0.2f), 0.05f)
            .SetEase(Ease.Linear))
            .Append(scoreText.transform.DOScale(originalScale, 0.1f)
            .SetEase(Ease.Linear));
    }
}
