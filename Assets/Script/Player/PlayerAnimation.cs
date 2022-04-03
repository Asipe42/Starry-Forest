using UnityEngine;

public class PlayerAnimation : BaseAnimation
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        PlayWalkAnimation(true);
    }

    public void PlayWalkAnimation(bool state)
    {
        base.SetAnimationClip(anim, "walk", state);
    }

    public void PlayJumpAnimation(bool state)
    {
        base.SetAnimationClip(anim, "jump", state);
    }

    public void PlayDownhillAnimation(bool state)
    {
        base.SetAnimationClip(anim, "downhill", state);
    }

    public void PlaySlidingAnimation(bool state)
    {
        base.SetAnimationClip(anim, "sliding", state);
    }

    public void SetAnimationClipSpeed(float speed)
    {
        base.SetAnimationClipSpeed(anim, speed);
    }
}
