using UnityEngine;

public class PlayerAnimation : BaseAnimation
{
    Animator anim;

    void Awake()
    {
        Initialize();
    }

    #region Initial Setting
    void Initialize()
    {
        anim = GetComponent<Animator>();
    }
    #endregion

    void Update()
    {
        CheckWalkAnimation();
        CheckDashAnimation();
    }

    #region Check Animation
    void CheckWalkAnimation()
    {
        PlayWalkAnimation(PlayerController.instance.onWalk);
    }

    void CheckDashAnimation()
    {
        if (PlayerController.instance.dashLevel > DashLevel.None)
            PlayDashAnimation(true);
        else
            PlayDashAnimation(false);
    }
    #endregion

    #region Play Animation
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
    #endregion

    /// <summary>
    /// player의 모든 animation clip의 speed를 speed로 조정한다.
    /// </summary>
    /// <param name="speed"></param>
    public void SetAnimationSpeed(float speed)
    {
        base.SetAnimationSpeed(anim, speed);
    }
}
