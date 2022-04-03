using UnityEngine;
using DG.Tweening;
using TMPro;

public class Score : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("Score")]
    public int score;

    [Space]
    [SerializeField] float shakeDuration = 0.5f;

    void Start()
    {
        CheckScore(0);
    }

    public void CheckScore(int score)
    {
        this.score += score;
        scoreText.text = this.score.ToString();
        scoreText.transform.DOShakeScale(shakeDuration);
    }
}
