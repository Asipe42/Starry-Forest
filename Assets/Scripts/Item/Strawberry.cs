using UnityEngine;
using DG.Tweening;

public class Strawberry : MonoBehaviour
{
    AudioClip strawberryClip;

    void Awake()
    {
        strawberryClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Dandelion");
    }

    void Start()
    {
        transform.DOMoveY(-0.02f, 3f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.instance.TakeItem(ItemType.Strawberry);
            SFXController.instance.PlaySFX(strawberryClip);
            Destroy(gameObject);
        }
    }
}
