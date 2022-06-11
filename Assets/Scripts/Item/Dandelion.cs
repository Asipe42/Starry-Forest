using UnityEngine;
using DG.Tweening;

public class Dandelion : MonoBehaviour
{
    AudioClip dandelionClip;

    void Awake()
    {
        dandelionClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Dandelion");
    }

    void Start()
    {
        transform.DOMoveY(-0.02f, 4f).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!PlayerController.instance.canFly)
            {
                PlayerController.instance.canFly = true;
                SFXController.instance.PlaySFX(dandelionClip);
                transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutElastic).OnComplete(() => Destroy(gameObject));
            }
        }
    }
}