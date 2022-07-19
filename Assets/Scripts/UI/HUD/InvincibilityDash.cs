using System.Collections;
using UnityEngine;
using DG.Tweening;

public class InvincibilityDash : MonoBehaviour
{
    [SerializeField] float slidingInDuration;
    [SerializeField] float slidingOutDuration;
    [SerializeField] float cooltime;
    [SerializeField] Transform slidingInPosition;
    [SerializeField] Transform slidingOutPosition;
    [SerializeField] Ease slidingInEase;
    [SerializeField] Ease slidingOutEase;

    AudioClip invincibilityDashClip;

    void Awake()
    {
        invincibilityDashClip = Resources.Load<AudioClip>("Audio/SFX/SFX_InvincibilityDash");
    }

    public IEnumerator PlaySlidingAnimation()
    {
        SFXController.instance.PlaySFX(invincibilityDashClip);

        SlidingUI(slidingInPosition.position, slidingInDuration, slidingInEase);

        yield return new WaitForSeconds(cooltime);

        SlidingUI(slidingOutPosition.position, slidingOutDuration, slidingOutEase);
    }

    void SlidingUI(Vector3 targetPosition, float duration, Ease ease)
    {
        transform.DOMove(targetPosition, duration).SetEase(ease);
    }
}
