using UnityEngine;

public class PlayerAnimation : BaseAnimation
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        PlayWalkAnimation(PlayerController.instance.onWalk);

        if (PlayerController.instance.dashLevel > DashLevel.None)
            PlayDashAnimation(true);
        else
            PlayDashAnimation(false);
    }

    public void PlayWalkAnimation(bool state = true)
    {
        base.SetAnimationClip(anim, "walk", state);
    }

    public void PlayJumpAnimation(bool state = true)
    {
        base.SetAnimationClip(anim, "jump", state);
    }

    public void PlayDownhillAnimation(bool state = true)
    {
        base.SetAnimationClip(anim, "downhill", state);
    }

    public void PlaySlidingAnimation(bool state = true)
    {
        base.SetAnimationClip(anim, "sliding", state);
    }

    public void PlayDashAnimation(bool state = true)
    {
        base.SetAnimationClip(anim, "dash", state);
    }

    public void PlayDeadAnimation(bool state = true)
    {
        base.SetAnimationClip(anim, "dead", state);
    }

    public void SetAnimationClipSpeed(float speed)
    {
        base.SetAnimationClipSpeed(anim, speed);
    }
}
