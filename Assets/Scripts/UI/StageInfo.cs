using UnityEngine;
using UnityEngine.UI;

public class StageInfo : MonoBehaviour
{
    [SerializeField] Text _mainText;
    [SerializeField] Text _subText;
    [SerializeField] string _mainString;
    [SerializeField] string _subString;
    [SerializeField] float[] _delay = { 1, 1.5f };

    Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        _mainText.text = _mainString;
        _subText.text = _subString;

        Invoke("AppearStageInfo", _delay[0]);
    }

    public void AppearStageInfo()
    {
        anim.SetTrigger(Definition.ANIM_APPEAR);

        Invoke("DisappearStageInfo", _delay[1]);
    }

    void DisappearStageInfo()
    {
        anim.SetTrigger(Definition.ANIM_DISAPPEAR);
    }
}
