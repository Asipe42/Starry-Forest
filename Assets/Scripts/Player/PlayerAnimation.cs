using UnityEngine;

public class PlayerAnimation : BaseAnimation
{
    Animator anim;
    bool onGround;

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
        CheckGround();
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

    void CheckGround()
    {
        if (this.onGround != PlayerController.instance.onGround)
        {
            this.onGround = PlayerController.instance.onGround;
            base.SetAnimationClip(anim, "onGround", this.onGround);
        }
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

    public void PlayFlyAnimation(bool state = true)
    {
        base.SetAnimationClip(anim, "fly", state);
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

    public void SetEveryAnimationParameter(bool state)
    {
        string[] animationParameter = { "walk", "jump", "downhill", "sliding", "dash", "fly" };

        foreach (var parameter in animationParameter)
        {
            base.SetAnimationClip(anim, parameter, state);
        }
    }
}
