using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AccelerationSpace
{ 
    public enum AccelerationLevel
    {
        None,
        One,
        Two,
        Three,
        Max
    }
}

namespace ClearGradeSpace
{
    public enum ClearGrade
    {
        Perfect,
        Excellent,
        Great,
        Good,
        Normal,
        Bad
    }
}

public class Definition : MonoBehaviour
{
    public const string WALK_CLIP = "walk";
    public const string JUMP_CLIP = "jump";
    public const string SLIDING_CLIP = "sliding";
    public const string ITEM_CLIP = "item";
    public const string THORN_CLIP = "thron"; // == vine clip
    public const string RECOVER_CLIP = "recover";
    public const string DOWNHILL_CLIP = "downhill";
    public const string DANDELION_CLIP = "dandelion";
    public const string DASH_CLIP = "dash";
    public const string DASH_LEVEL_UP_CLIP = "doubleDash";
    public const string POP_UP_CLIP = "popup";

    public const float WALK_VOLUME = 0.3f;
    public const float JUMP_VOLUME = 0.6f;
    public const float SLIDING_VOLUME = 0.3f;
    public const float ITEM_VOLUME = 0.6f;
    public const float THORN_VOLUME = 0.6f;
    public const float RECOVER_VOLUME = 0.8f;
    public const float DOWNHILL_VOLUME = 0.3f;
    public const float DANDELION_VOLUME = 0.4f;
    public const float DASH_VOLUME = 0.6f;
    public const float DASH_LEVEL_UP_VOLUME = 0.8f;
    public const float POP_UP_VOLUME = 1.0f;

    public const string ANIM_STANDING = "onStand";
    public const string ANIM_JUMP = "doJump";
    public const string ANIM_SLIDE = "doSlide";
    public const string ANIM_DOWNHILL = "doDownhill";
    public const string ANIM_FLY = "onFly";
    public const string ANIM_DASH = "onDash";
    public const string ANIM_APPEAR = "appear";
    public const string ANIM_DISAPPEAR = "disappear";
    public const string ANIM_POP_UP = "PopUp";
    public const string ANIM_POP_DOWN = "PopDown";
    public const string ANIM_KNOCKDOWN = "onDead";

    public const string VFX_DUST = "dust";
    public const string VFX_TAKE_ITEM = "takeItem";
    public const string VFX_RECOVER = "recover";
    public const string VFX_DANDELION = "dandelion";
    public const string VFX_DASH = "dash";
    public const string VFX_KNOCKDOWN = "knockdown";
}
