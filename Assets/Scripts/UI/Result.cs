using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Result : MonoBehaviour
{
    enum ResultMenu
    {
        None,
        Retry,
        Next,
        Maximam
    }

    enum EffectColor
    {
        Gold,
        Silver,
        Bronze
    }

    [Header("Score")]
    [SerializeField] Text _scoreRatioText;
    [SerializeField] int _minimuScoreValue_1;
    [SerializeField] Image _scoreEffectImage;
    bool _printedScore;

    [Header("Time")]
    [SerializeField] Text _clearTimeText;
    [SerializeField] float[] _clearTimeTable_1;
    [SerializeField] Image _clearTimeEffectImage;
    bool _printedTime;

    [Header("Hp")]
    [SerializeField] Text _hpText;
    [SerializeField] Image _hpEffectImage;

    [Header("Grade")]
    [SerializeField] Text _gradeText;
    [SerializeField] Image _gradeImage;
    [SerializeField] Sprite[] _gradeSprites;
    [SerializeField] Image _gradeEffectImage;
    bool _printedGrade;
    ClearGradeSpace.ClearGrade _myGrade;

    [Space]
    [SerializeField] Color[] _effectColors;

    [Header("Selected Menu")]
    [SerializeField] KeyCode[] leftKeys;
    [SerializeField] KeyCode[] rightKeys;
    [SerializeField] Image _RetryButtonImage;
    [SerializeField] Image _NextButtonImage;
    [SerializeField] Color _SelectedColor;
    [SerializeField] Color _defaultColor;
    [SerializeField] AudioClip _menuChangeClip;
    ResultMenu _mySelectedMenu;
    bool _onSelectMenu;

    [Header("Box Anim")]
    [SerializeField] Animator _resultBoxAnim;
    [SerializeField] GameObject _guideBox;
    [SerializeField] Animator _guideBoxAnim;
    public static bool _onGuideBox;

    AudioSource audioSource;

    public PlayableDirector playableDirector;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        _mySelectedMenu = ResultMenu.None;
    }

    void Update()
    {
        if (_onSelectMenu)
            Select();
    }

    public void StartDialog(int index)
    {
        //GameManager.instance.UIManagerInstance.DialogInstance.SetCondition(index);
    }

    public void SetResult()
    {
        PlayerController tempLogic = GameManager.instance.PlayerControllerInstance;

        int hp = tempLogic._hp;
        int maxHp = tempLogic._maxHp;

        int score = GameManager.instance.UIManagerInstance.ScoreInstance.totalScore;

        float clearTime = GameManager.instance.StageManagerInstance.clearTime;

        SetClearTime(clearTime);

        SetHp(hp, maxHp);

        CalcGrade(score, clearTime, hp, maxHp);

        playableDirector.Play();
    }

    void SetScore(float ratio)
    {
        _scoreRatioText.text = ratio.ToString() + "%";
    }

    void SetClearTime(float time)
    {
        int minute = 0;
        int second = 0;

        while (time > 60)
        {
            minute++;
            time -= 60;
        }

        second = (int)time;

        string timeStr = string.Format("{0:00}:{1:00}", minute, second);

        _clearTimeText.text = timeStr;
    }

    void SetHp(int hp, int maxHp)
    {
        _hpText.text = hp + " / " + maxHp;
    }

    void CalcGrade(int score, float clearTime, int hp, int maxHp)
    {
        int totalWeight = 0;

        totalWeight += SetScoreWeight(score);
        totalWeight += SetClearTimeWeight(clearTime);
        totalWeight += SetHpWeight(hp, maxHp);

        Debug.Log(totalWeight);

        if (totalWeight <= 30)
        {
            _myGrade = ClearGradeSpace.ClearGrade.Perfect;
        }
        else if (totalWeight <= 27)
        {
            _myGrade = ClearGradeSpace.ClearGrade.Excellent;
        }
        else if (totalWeight <= 23)
        {
            _myGrade = ClearGradeSpace.ClearGrade.Great;
        }
        else if (totalWeight <= 19)
        {
            _myGrade = ClearGradeSpace.ClearGrade.Good;
        }
        else if (totalWeight <= 15)
        {
            _myGrade = ClearGradeSpace.ClearGrade.Normal;
        }
        else
        {
            _myGrade = ClearGradeSpace.ClearGrade.Bad;
        }

        SetGrade(_myGrade);
    }

    int SetScoreWeight(int score)
    {
        int scoreWeight;

        float totalItemCount = (float)GameManager.instance.StageManagerInstance.totalItemCount;

        float takedItemCount = (float)score;

        int itemRatio = (int)((takedItemCount / totalItemCount) * 100);

        if (itemRatio > 90)
        {
            scoreWeight = 10;
            _scoreEffectImage.color = _effectColors[(int)EffectColor.Gold];
        }
        else if (itemRatio > 80)
        {
            scoreWeight = 9;
            _scoreEffectImage.color = _effectColors[(int)EffectColor.Silver];
        }
        else if (itemRatio > 70)
        {
            scoreWeight = 8;
            _scoreEffectImage.color = _effectColors[(int)EffectColor.Silver];
        }
        else if (itemRatio > 60)
        {
            scoreWeight = 7;
            _scoreEffectImage.color = _effectColors[(int)EffectColor.Bronze];
        }
        else if (itemRatio > 50)
        {
            scoreWeight = 6;
            _scoreEffectImage.color = _effectColors[(int)EffectColor.Bronze];
        }
        else
        {
            scoreWeight = 5;
            _scoreEffectImage.color = _effectColors[(int)EffectColor.Bronze];
        }

        SetScore(itemRatio);

        return scoreWeight;
    }

    int SetClearTimeWeight(float clearTime)
    {
        int clearTimeWeight = 0;

        if (clearTime < _clearTimeTable_1[0])
        {
            clearTimeWeight = 10;
            _clearTimeEffectImage.color = _effectColors[(int)EffectColor.Gold];
        }
        else if (clearTime < _clearTimeTable_1[1])
        {
            clearTimeWeight = 8;
            _clearTimeEffectImage.color = _effectColors[(int)EffectColor.Silver];
        }
        else if (clearTime < _clearTimeTable_1[2])
        {
            clearTimeWeight = 6;
            _clearTimeEffectImage.color = _effectColors[(int)EffectColor.Silver];
        }
        else if (clearTime < _clearTimeTable_1[3])
        {
            clearTimeWeight = 4;
            _clearTimeEffectImage.color = _effectColors[(int)EffectColor.Bronze];
        }
        else if (clearTime < _clearTimeTable_1[4])
        {
            clearTimeWeight = 2;
            _clearTimeEffectImage.color = _effectColors[(int)EffectColor.Bronze];
        }
        else
        {
            clearTimeWeight = 0;
            _clearTimeEffectImage.color = _effectColors[(int)EffectColor.Bronze];
        }

        return clearTimeWeight;
    }

    int SetHpWeight(int hp, int maxHp)
    {
        int scoreWeight = 0;

        if (maxHp == hp)
        {
            scoreWeight = 10;
            _hpEffectImage.color = _effectColors[(int)EffectColor.Gold];
        }
        else if (maxHp / 2 <= hp)
        {
            scoreWeight = 8;
            _hpEffectImage.color = _effectColors[(int)EffectColor.Silver];
        }
        else
        {
            scoreWeight = 6;
            _hpEffectImage.color = _effectColors[(int)EffectColor.Bronze];
        }

        return scoreWeight;
    }

    void SetGrade(ClearGradeSpace.ClearGrade grade)
    {
        string gradeStr = null;

        switch (grade)
        {
            case ClearGradeSpace.ClearGrade.Perfect:
                gradeStr = "참 잘했어요!";
                _gradeImage.sprite = _gradeSprites[(int)ClearGradeSpace.ClearGrade.Perfect];
                _gradeEffectImage.color = _effectColors[(int)EffectColor.Gold];
                break;
            case ClearGradeSpace.ClearGrade.Excellent:
                gradeStr = "멋져요!";
                _gradeImage.sprite = _gradeSprites[(int)ClearGradeSpace.ClearGrade.Excellent];
                _gradeEffectImage.color = _effectColors[(int)EffectColor.Gold];
                break;
            case ClearGradeSpace.ClearGrade.Great:
                gradeStr = "잘했어요";
                _gradeImage.sprite = _gradeSprites[(int)ClearGradeSpace.ClearGrade.Great];
                _gradeEffectImage.color = _effectColors[(int)EffectColor.Silver];
                break;
            case ClearGradeSpace.ClearGrade.Good:
                gradeStr = "좋아요";
                _gradeImage.sprite = _gradeSprites[(int)ClearGradeSpace.ClearGrade.Good];
                _gradeEffectImage.color = _effectColors[(int)EffectColor.Silver];
                break;
            case ClearGradeSpace.ClearGrade.Normal:
                gradeStr = "더 열심히";
                _gradeImage.sprite = _gradeSprites[(int)ClearGradeSpace.ClearGrade.Normal];
                _gradeEffectImage.color = _effectColors[(int)EffectColor.Bronze];
                break;
            case ClearGradeSpace.ClearGrade.Bad:
                gradeStr = "노력하세요!";
                _gradeImage.sprite = _gradeSprites[(int)ClearGradeSpace.ClearGrade.Bad];
                _gradeEffectImage.color = _effectColors[(int)EffectColor.Bronze];
                break;
        }

        _gradeText.text = gradeStr;
    }

    public void OnSelectMenu()
    {
        _onSelectMenu = true;
    }

    void Select()
    {
        ChangeMenu();
        ShowSelectedMenu();
        Decision();
    }

    void ChangeMenu()
    {
        if (Input.GetKeyDown(leftKeys[0]) || Input.GetKeyDown(leftKeys[1]))
        {
            _mySelectedMenu--;

            if (_mySelectedMenu <= ResultMenu.None)
            {
                _mySelectedMenu = ResultMenu.Retry;
            }

            audioSource.PlayOneShot(_menuChangeClip);
        }
        else if (Input.GetKeyDown(rightKeys[0]) || Input.GetKeyDown(rightKeys[1]))
        {
            _mySelectedMenu++;

            if (_mySelectedMenu >= ResultMenu.Maximam)
            {
                _mySelectedMenu = ResultMenu.Next;
            }

            audioSource.PlayOneShot(_menuChangeClip);
        }
    }

    void ShowSelectedMenu()
    {
        switch (_mySelectedMenu)
        {
            case ResultMenu.Retry:
                _RetryButtonImage.color = _SelectedColor;
                _NextButtonImage.color = _defaultColor;
                break;

            case ResultMenu.Next:
                _NextButtonImage.color = _SelectedColor;
                _RetryButtonImage.color = _defaultColor; 
                break;
        }
    }

    void Decision()
    {
        if (Input.GetButtonDown("Submit"))
        {
            switch (_mySelectedMenu)
            {
                case ResultMenu.Retry:
                    SelectRetry();
                    break;

                case ResultMenu.Next:
                    SelectNext();
                    break;
            }
        }
    }

    void SelectRetry()
    {
        if (!_onGuideBox)
        {
            GameManager.instance.AudioManagerInstance.PlaySFX(Definition.POP_UP_CLIP);

            _resultBoxAnim.SetTrigger(Definition.ANIM_POP_DOWN);

            _guideBox.SetActive(true);

            _onGuideBox = true;

            _guideBoxAnim.SetTrigger(Definition.ANIM_POP_UP);
        }
    }

    void SelectNext()
    {
        GameManager.instance.StageManagerInstance.Victory();
    }
}
