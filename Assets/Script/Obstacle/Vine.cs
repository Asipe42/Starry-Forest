using UnityEngine;
using DG.Tweening;

public class Vine : Obstacle
{
    [Space]
    [SerializeField] AudioClip appearClip;

    [Space]
    public bool onAppear;

    void Start()
    {
        appearClip = Resources.Load<AudioClip>("Audio/SFX/SFX_VineAppear");
    }

    public void Appear()
    {
        if (onAppear)
            return;

        onAppear = true;
        AudioManager.instance.PlaySFX(appearClip);

        transform.DOMoveY(-0.3f, 0.7f).SetEase(Ease.OutSine);
    }
}
