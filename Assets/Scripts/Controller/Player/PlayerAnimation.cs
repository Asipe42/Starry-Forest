using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        AllocateComponent();
    }

    void AllocateComponent()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayAnimationClip(string clipName, bool state)
    {
        switch (clipName)
        {
            case Definition.ANIM_STANDING:
            case Definition.ANIM_JUMP:
            case Definition.ANIM_SLIDE:
            case Definition.ANIM_DOWNHILL:
            case Definition.ANIM_FLY:
            case Definition.ANIM_DASH:
            case Definition.ANIM_KNOCKDOWN:
                anim.SetBool(clipName, state);
                break;
            default:
                Debug.Log("Name is Fault");
                break;
        }
    }

    public void PlayAnimationClip(string clipName) // Trigger
    {
        ;
    }
}
