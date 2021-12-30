using UnityEngine;
using UnityEngine.UI;

public class StageInfo : MonoBehaviour
{
    [SerializeField] Text _text;
    [SerializeField] string _stageTitle;
    [SerializeField] float[] _delay = { 1, 1.5f };

    [Header("SFX")]
    [SerializeField] AudioClip _appearClip;
    [SerializeField] AudioClip _disappearClip;
    AudioSource audio;
    Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        _text.text = _stageTitle;

        Invoke("AppearStageInfo", _delay[0]);
    }

    public void AppearStageInfo()
    {
        audio.pitch = 2f;
        audio.PlayOneShot(_appearClip);

        anim.SetTrigger(Definition.ANIM_APPEAR);

        Invoke("DisappearStageInfo", _delay[1]);
    }

    void DisappearStageInfo()
    {
        audio.pitch = 1f;
        audio.PlayOneShot(_disappearClip);

        anim.SetTrigger(Definition.ANIM_DISAPPEAR);
    }
}
