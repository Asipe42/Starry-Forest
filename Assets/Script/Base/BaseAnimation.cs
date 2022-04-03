using UnityEngine;

public class BaseAnimation : MonoBehaviour
{
    protected void SetAnimationClip(Animator anim, string name, bool state)
    {
        anim.SetBool(name, state);
    }

    protected void SetAnimationClip(Animator anim, string name)
    {
        anim.SetTrigger(name);
    }

    public void SetAnimationClipSpeed(Animator anim, float speed)
    {
        anim.speed = speed;
    }
}
