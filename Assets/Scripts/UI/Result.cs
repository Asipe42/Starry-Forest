using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Result : MonoBehaviour
{
    enum ClearGrade
    {
        Perfect,
        Excellent, 
        Great,
        Good,
        Normal,
        Bad
    }

    enum ResultMenu
    {
        None,
        Retry,
        Next,
        Maximam
    }

    [Header("Score")]
    [SerializeField] Text _scoreText;
    [SerializeField] int _minimuScoreValue_1;
    bool _printedScore;

    [Header("Time")]
    [SerializeField] Text _clearTimeText;
    [SerializeField] float[] _clearTimeTable_1;
    bool _printedTime;

    [Header("Grade")]
    [SerializeField] Text _gradeText;
    [SerializeField] Image _gradeImage;
    [SerializeField] Sprite[] _gradeSprites;
    bool _printedGrade;
    ClearGrade _myGrade;

    [Header("Selected Menu")]
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    [SerializeField] Image _RetryButtonImage;
    [SerializeField] Image _NextButtonImage;
    [SerializeField] Color _SelectedColor;
    [SerializeField] Color _defaultColor;
    [SerializeField] AudioClip _menuChangeClip;
    ResultMenu _mySelectedMenu;
    bool _onSelectMenu;

    AudioSource audioSource;

    public PlayableDirector playableDirector;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        _mySelectedMenu = ResultMenu.Retry;
    }

    void Update()
    {
        if (_onSelectMenu)
            Select();
    }

    public void StartDialog(int index)
    {
        GameManager.instance.UIManagerInstance.DialogInstance.SetCondition(index);
    }

    public void SetResult()
    {
        PlayerController tempLogic = GameManager.instance.PlayerControllerInstance;

        int hp = tempLogic._hp;
        int maxHp = tempLogic._maxHp;

        int score = GameManager.instance.UIManagerInstance.ScoreInstance.totalScore;

        float clearTime = GameManager.instance.StageManagerInstance.clearTime;

        SetScore(score);

        SetClearTime(clearTime);

        CalcGrade(score, clearTime, hp, maxHp);

        playableDirector.Play();
    }

    void SetScore(float score)
    {
        _scoreText.text = score.ToString();
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

    void CalcGrade(int score, float clearTime, int hp, int maxHp)
    {
        int totalWeight = 0;

        totalWeight += SetScoreWeight(score);
        totalWeight += SetClearTimeWeight(clearTime);
        totalWeight += SetHpWeight(hp, maxHp);

        if (totalWeight <= 30)
        {
            _myGrade = ClearGrade.Perfect;
            _gradeImage.sprite = _gradeSprites[(int)ClearGrade.Perfect];
        }
        else if (totalWeight <= 27)
        {
            _myGrade = ClearGrade.Excellent;
            _gradeImage.sprite = _gradeSprites[(int)ClearGrade.Excellent];
        }
        else if (totalWeight <= 23)
        {
            _myGrade = ClearGrade.Great;
            _gradeImage.sprite = _gradeSprites[(int)ClearGrade.Great];
        }
        else if (totalWeight <= 19)
        {
            _myGrade = ClearGrade.Good;
            _gradeImage.sprite = _gradeSprites[(int)ClearGrade.Good];
        }
        else if (totalWeight <= 15)
        {
            _myGrade = ClearGrade.Normal;
            _gradeImage.sprite = _gradeSprites[(int)ClearGrade.Normal];
        }
        else
        {
            _myGrade = ClearGrade.Bad;
            _gradeImage.sprite = _gradeSprites[(int)ClearGrade.Bad];
        }

        SetGrade(_myGrade);
    }

    int SetScoreWeight(int score)
    {
        int scoreWeight = 10;

        if (score > _minimuScoreValue_1)
            scoreWeight = 10;
        else
            scoreWeight = 9;

        return scoreWeight;
    }

    int SetClearTimeWeight(float clearTime)
    {
        int clearTimeWeight = 0;

        if (clearTime < _clearTimeTable_1[0])
        {
            clearTimeWeight = 10;
        }
        else if (clearTime < _clearTimeTable_1[1])
        {
            clearTimeWeight = 8;
        }
        else if (clearTime < _clearTimeTable_1[2])
        {
            clearTimeWeight = 6;
        }
        else if (clearTime < _clearTimeTable_1[3])
        {
            clearTimeWeight = 4;
        }
        else if (clearTime < _clearTimeTable_1[4])
        {
            clearTimeWeight = 2;
        }
        else
        {
            clearTimeWeight = 0;
        }

        return clearTimeWeight;
    }

    int SetHpWeight(int hp, int maxHp)
    {
        int scoreWeight = 0;

        if (maxHp == hp)
        {
            scoreWeight = 10;
        }
        else if (maxHp / 2 <= hp)
        {
            scoreWeight = 8;
        }
        else
        {
            scoreWeight = 6;
        }

        return scoreWeight;
    }

    void SetGrade(ClearGrade grade)
    {
        string gradeStr = null;

        switch (grade)
        {
            case ClearGrade.Perfect:
                gradeStr = "참 잘했어요!";
                break;
            case ClearGrade.Excellent:
                gradeStr = "멋져요!";
                break;
            case ClearGrade.Great:
                gradeStr = "잘했어요";
                break;
            case ClearGrade.Good:
                gradeStr = "좋아요";
                break;
            case ClearGrade.Normal:
                gradeStr = "더 열심히";
                break;
            case ClearGrade.Bad:
                gradeStr = "노력하세요!";
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
        if (Input.GetKeyDown(left))
        {
            _mySelectedMenu--;

            if (_mySelectedMenu == ResultMenu.None)
            {
                _mySelectedMenu = ResultMenu.Retry;
            }

            audioSource.PlayOneShot(_menuChangeClip);
        }
        else if (Input.GetKeyDown(right))
        {
            _mySelectedMenu++;

            if (_mySelectedMenu == ResultMenu.Maximam)
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
        Debug.Log("Selected Retry");
    }

    void SelectNext()
    {
        Debug.Log("Selected Next");
    }
}
