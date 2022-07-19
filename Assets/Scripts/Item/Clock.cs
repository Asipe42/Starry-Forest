using UnityEngine;
using DG.Tweening;

public class Clock : MonoBehaviour
{
    [SerializeField] float time;

    AudioClip clockClip;

    void Start()
    {
        PlayMoveAnimation();
    }

    #region Initial Setting
    void GetAudioClip()
    {
        clockClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Clock");
    }
    #endregion

    void PlayMoveAnimation()
    {
        transform.DOMoveY(-0.02f, 3f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    void AddTime()
    {
        UIManager.instance.rank.CurrentTime += time;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SFXController.instance.PlaySFX(clockClip);
            AddTime();
            Destroy(gameObject);
        }
    }
}
