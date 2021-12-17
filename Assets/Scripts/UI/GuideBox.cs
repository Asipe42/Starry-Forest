using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideBox : MonoBehaviour
{
    [SerializeField] Animator _resultBoxAnim;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Escape();
        Retry();
    }

    void Escape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.AudioManagerInstance.PlaySFX(Definition.POP_UP_CLIP);

            anim.SetTrigger(Definition.ANIM_POP_DOWN);

            _resultBoxAnim.SetTrigger(Definition.ANIM_POP_UP);

            Result._onGuideBox = false;
        }
    }

    void Retry()
    {
        if (Input.GetButtonDown("Submit"))
        {
            GameManager.instance.StageManagerInstance.Retry();
        }
    }

    void Off()
    {
        gameObject.SetActive(false);
    }
}
