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
        PlayMoveAnimation();
    }

    void PlayMoveAnimation()
    {
        transform.DOMoveY(-0.02f, 3f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.instance.TakeItem(ItemType.Dandelion);
            SFXController.instance.PlaySFX(dandelionClip);
            Destroy(gameObject);
        }
    }
}