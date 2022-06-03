using UnityEngine;
using DG.Tweening;

public class Mushroom : MonoBehaviour
{
    [SerializeField] int score = 1;

    public float delay;

    void Awake()
    {
        // delay setting
    }

    void Start()
    {
        PlayAnimation();
    }

    void PlayAnimation()
    {
        transform.DOLocalMoveY(transform.position.y + 0.2f, 1f).SetEase(Ease.InOutQuad)
                                                       .SetLoops(-1, LoopType.Yoyo)
                                                       .SetDelay(delay);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out PlayerController pc))
            {
                pc.TakeItem(score);
            }

            transform.DOKill();
            Destroy(gameObject);
        }
    }
}
