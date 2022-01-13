using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum CharacterIndex
{
    None,
    Dal,
    Daram,
}

[System.Serializable]
class CharacterInfo
{
    public Sprite _portait;
    public Vector3 _scale;
    public string _name;
}

[System.Serializable]
class ScriptInfo
{
    [TextArea] public string _script;
    public CharacterIndex _characterIndex;
    public bool _onPortriat;
    public bool _onOption;
    public int _optionCount;
    public bool[] _event = new bool[3];
}

public class Dialog : MonoBehaviour
{
    enum OptionIndex
    {
        first,
        Second,
        Third
    }

    [Header("Core")]
    [SerializeField] CharacterInfo[] _characterInfo;
    [SerializeField] ScriptInfo[] _scriptInfo;
    [SerializeField] int _startScriptNumber;
    [SerializeField] int _endScriptNumber;
    [SerializeField] string _nextSceneName;
    int _nowScriptNumber = 0;
    bool _onEnd;

    [Header("Portrait")]
    [SerializeField] Image _portraitImage;
    [SerializeField, Range(0, 1)] float _portraitFadeSpeed = 0.1f;
    [SerializeField] float _portraitFadeDelay = 0.05f;
    int _nowCharacterIndex = -1;
    float _portraitImageColorAlphaValue;
    IEnumerator FadePortraitCorutine;
    OptionIndex _nowOptionIndex = OptionIndex.first;
    
    [Header("Texts")]
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] Text _typingText;
    [SerializeField] float _typingSpeed;
    [SerializeField] AudioClip _messageClip;
    [SerializeField] AudioClip _selectClip; 
    [SerializeField] Image[] _selectBar;
    [SerializeField] AudioSource channel_1;
    [SerializeField] AudioSource channel_2;
    int _nowOptionCount = 0;
    bool _onSelect;

    [Header("etc")]
    [SerializeField] Image _arrowImage;

    private void Update()
    {
        NextScript();
        OnSelect();
    }

    public void StartScript()
    {
        PrintScript(_startScriptNumber, _endScriptNumber);
        _onEnd = true;

    }

    private void NextScript()
    {
        if (Input.GetButtonDown("Submit") && _arrowImage.enabled)
        {
            if (_nowScriptNumber < _endScriptNumber)
            {
                _nowScriptNumber++;

                ScriptInfo tempScriptInfo = _scriptInfo[_nowScriptNumber];

                StartCoroutine(TypingText(tempScriptInfo._script, _typingSpeed, tempScriptInfo._onPortriat, (int)tempScriptInfo._characterIndex, tempScriptInfo._onOption, tempScriptInfo._optionCount));
            }
            else if (_onEnd)
            {
                LoadingSceneController.LoadScene(_nextSceneName);
            }
            else
            {
                ContinueGame();
            }
        }
    }

    void ContinueGame()
    {
        GameManager.instance.StageManagerInstance.end = false;
        GameManager.instance.StageManagerInstance.stop = false;
        GameManager.instance.UIManagerInstance.SetHUD(true);
    }

    private void PrintScript(int start, int end) // need to change portrait logic
    {
        _nowScriptNumber = start;
        _endScriptNumber = end;

        ScriptInfo tempScriptInfo = _scriptInfo[_nowScriptNumber];

        StartCoroutine(TypingText(tempScriptInfo._script, _typingSpeed, tempScriptInfo._onPortriat, (int)tempScriptInfo._characterIndex, tempScriptInfo._onOption, tempScriptInfo._optionCount));
    }

    IEnumerator TypingText(string message, float speed, bool portrait, int characterIndex, bool option, int optionCount)
    {
        _arrowImage.enabled = false;

        if (characterIndex >= 0)
        {
            _name.text = _characterInfo[characterIndex]._name;
        }
        else
        {
            _name.text = "";
            Debug.Log("empty name");
        }

        if (_nowCharacterIndex != characterIndex)
        {
            _portraitImageColorAlphaValue = 0f;
            _portraitImage.color = new Color(1, 1, 1, 0);

            if (FadePortraitCorutine != null)
            {
                StopCoroutine(FadePortraitCorutine);
            }

            FadePortraitCorutine = FadePortrait();
            StartCoroutine(FadePortraitCorutine);
        }

        _nowCharacterIndex = characterIndex;

        if (portrait)
        {
            _portraitImage.enabled = true;

            _portraitImage.sprite = _characterInfo[characterIndex]._portait;
            _portraitImage.rectTransform.localScale = _characterInfo[characterIndex]._scale;
        }
        else
        {
            _portraitImage.enabled = false;
        }

        foreach (var bar in _selectBar)
        {
            bar.enabled = false;
        }

        if (option)
        {
            _selectBar[0].enabled = true;
            _nowOptionIndex = OptionIndex.first;

            _typingText.text = message;

            _nowOptionCount = optionCount;
            OnSelect();

            _onSelect = true;
        }
        else
        {
            for (int i = 0; i < message.Length; i++)
            {
                if (!channel_1.isPlaying)
                {
                    channel_1.clip = _messageClip;
                    channel_1.Play();
                }

                _typingText.text = message.Substring(0, i + 1);
                yield return new WaitForSeconds(speed);
            }

            channel_1.Stop();
            _arrowImage.enabled = true;
        }
    }

    IEnumerator FadePortrait()
    {
        while (_portraitImageColorAlphaValue <= 1)
        {
            _portraitImageColorAlphaValue += _portraitFadeSpeed;

            _portraitImage.color = new Color(1, 1, 1, _portraitImageColorAlphaValue);

            yield return new WaitForSeconds(_portraitFadeDelay);
        }
    }

    void OnSelect()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            channel_2.PlayOneShot(_selectClip);

            switch (_nowOptionIndex)
            {
                case OptionIndex.first:
                    if (_nowOptionCount >= 3)
                    {
                        _nowOptionIndex = OptionIndex.Third;
                    }
                    else if (_nowOptionCount >= 2)
                    {
                        _nowOptionIndex = OptionIndex.Second;
                    }
                    else
                    {
                        ;
                    }
                    break;
                case OptionIndex.Second:
                    if (_nowOptionCount >= 3)
                    {
                        _nowOptionIndex = OptionIndex.first;
                    }
                    else if (_nowOptionCount >= 2)
                    {
                        _nowOptionIndex = OptionIndex.first;
                    }
                    break;
                case OptionIndex.Third:
                    if (_nowOptionCount >= 3)
                    {
                        _nowOptionIndex = OptionIndex.Second;
                    }
                    break;
            }
            ConvertSelectBar();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            channel_2.PlayOneShot(_selectClip);

            switch (_nowOptionIndex)
            {
                case OptionIndex.first:
                    if (_nowOptionCount >= 3)
                    {
                        _nowOptionIndex = OptionIndex.Second;
                    }
                    else if (_nowOptionCount >= 2)
                    {
                        _nowOptionIndex = OptionIndex.Second;
                    }
                    else
                    {
                        ;
                    }
                    break;
                case OptionIndex.Second:
                    if (_nowOptionCount >= 3)
                    {
                        _nowOptionIndex = OptionIndex.Third;
                    }
                    else if (_nowOptionCount >= 2)
                    {
                        _nowOptionIndex = OptionIndex.first;
                    }
                    break;
                case OptionIndex.Third:
                    if (_nowOptionCount >= 3)
                    {
                        _nowOptionIndex = OptionIndex.first;
                    }
                    break;
            }
            ConvertSelectBar();
        }
    }

    void ConvertSelectBar()
    {
        foreach (var bar in _selectBar)
        {
            bar.enabled = false;
        }

        switch (_nowOptionIndex)
        {
            case OptionIndex.first:
                _selectBar[0].enabled = true;
                break;
            case OptionIndex.Second:
                _selectBar[1].enabled = true;
                break;
            case OptionIndex.Third:
                _selectBar[2].enabled = true;
                break;
            default:
                break;
        }
    }
}
