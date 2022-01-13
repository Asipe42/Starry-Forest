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
    public const string TAKE_ITEM_CLIP = "item";
    public const string THORN_CLIP = "thron"; // == vine clip
    public const string RECOVER_CLIP = "recover";
    public const string DOWNHILL_CLIP = "downhill";
    public const string DANDELION_CLIP = "dandelion";
    public const string DASH_CLIP = "dash";
    public const string DASH_UPGRADE_CLIP = "doubleDash";
    public const string POP_UP_CLIP = "popup";
    public const string SELECT_CLIP = "select";


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
    public const float SELECT_VOLUME = 1.0f;

    public const string ANIM_STANDING = "idle";
    public const string ANIM_JUMP = "jump";
    public const string ANIM_SLIDE = "slide";
    public const string ANIM_DOWNHILL = "downhill";
    public const string ANIM_FLY = "fly";
    public const string ANIM_DASH = "dash";
    public const string ANIM_KNOCKDOWN = "dead";

    public const string ANIM_APPEAR = "appear";
    public const string ANIM_DISAPPEAR = "disappear";
    public const string ANIM_POP_UP = "popup";
    public const string ANIM_POP_DOWN = "popdown";
    public const string ANIM_SELECTED_RETRY = "selected_retry";
    public const string ANIM_SELECTED_NEXT = "selected_next";

    public const string VFX_DUST = "dust";
    public const string VFX_TAKE_ITEM = "takeItem";
    public const string VFX_RECOVER = "recover";
    public const string VFX_DANDELION = "dandelion";
    public const string VFX_DASH = "dash";
    public const string VFX_KNOCKDOWN = "knockdown";
}
