using UnityEngine.UI;
using UnityEngine;

public class SpiderWebScreen : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        Initialize();
        SubscribeEvent();
    }

    #region Initiali Setting
    void Initialize()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void SubscribeEvent()
    {
        PlayerController.WebDebuffEvent -= PlaySpiderWebScreenAnimation;
        PlayerController.WebDebuffEvent += PlaySpiderWebScreenAnimation;
    }
    #endregion

    void PlaySpiderWebScreenAnimation()
    {
        anim.SetTrigger("spiderWeb");
    }
}
