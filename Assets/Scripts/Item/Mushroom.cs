using UnityEngine;
using DG.Tweening;

public class Mushroom : MonoBehaviour
{
    [SerializeField] int score = 1;

    public float delay;

    void Start()
    {
        transform.DOLocalMoveY(0.1f, 2f).SetEase(Ease.InOutQuad)
                                         .SetLoops(-1, LoopType.Yoyo)
                                         .SetDelay(delay);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeItem(score);

            transform.DOKill();
            Destroy(gameObject);
        }
    }
}
