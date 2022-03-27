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
        SetAnimationClip(anim, "walk", state);
    }

    public void PlayJumpAnimation(bool state)
    {
        SetAnimationClip(anim, "jump", state);
    }

    public void PlayDownhillAnimation(bool state)
    {
        SetAnimationClip(anim, "downhill", state);
    }

    public void PlaySlidingAnimation(bool state)
    {
        SetAnimationClip(anim, "sliding", state);
    }
}
