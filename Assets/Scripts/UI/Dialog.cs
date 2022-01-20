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
    public bool _onEnd;
}

[System.Serializable]
struct ScriptIndex
{
    public int _startIndex;
    public int _endIndex;
}

public class Dialog : MonoBehaviour
{
    [Header("Core")]
    [SerializeField] CharacterInfo[] _characterInfo;
    [SerializeField] ScriptInfo[] _scriptInfo;
    [SerializeField] ScriptIndex[] _scriptIndex;
    [SerializeField] string _nextSceneName;
    IEnumerator TypingCorutine;
    int _startScriptNumber;
    int _endScriptNumber;
    int _nowScriptNumber = 0;
    int _scriptIndexNumber = 0;
    bool _onEnd;

    [Header("Portrait")]
    [SerializeField] Image _portraitImage;
    [SerializeField, Range(0, 1)] float _portraitFadeSpeed = 0.1f;
    [SerializeField] float _portraitFadeDelay = 0.05f;
    int _nowCharacterIndex = -1;
    float _portraitImageColorAlphaValue;
    IEnumerator FadePortraitCorutine;
    
    [Header("Texts")]
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] Text _typingText;
    [SerializeField] float _typingSpeed;
    [SerializeField] AudioClip _messageClip;
    [SerializeField] AudioSource audioChannel;

    [Header("etc")]
    [SerializeField] Image _arrowImage;

    private void Update()
    {
        NextScript();
    }

    void Start()
    {
        _startScriptNumber = _scriptIndex[_scriptIndexNumber]._startIndex;
        _endScriptNumber = _scriptIndex[_scriptIndexNumber]._endIndex;
    }

    public void StartScript()
    {
        PrintScript(_startScriptNumber, _endScriptNumber);
    }

    private void NextScript()
    {
        if (Input.GetButtonDown("Submit") && _arrowImage.enabled)
        {
            if (_nowScriptNumber < _endScriptNumber)
            {
                _nowScriptNumber++;

                ScriptInfo tempScriptInfo = _scriptInfo[_nowScriptNumber];
                StartCoroutine(TypingText(tempScriptInfo._script, _typingSpeed, tempScriptInfo._onPortriat, (int)tempScriptInfo._characterIndex, tempScriptInfo._onEnd));
            }
            else if (!_onEnd)
            {
                _scriptIndexNumber++;
                _startScriptNumber = _scriptIndex[_scriptIndexNumber]._startIndex;
                _endScriptNumber = _scriptIndex[_scriptIndexNumber]._endIndex;

                gameObject.SetActive(false);
                TimelineController.instant.ContinueTimeline();
            }
            else if (_onEnd)
            {
                LoadingSceneController.LoadScene(_nextSceneName);
            }
        }
    }

    void PrintScript(int start, int end)
    {
        _nowScriptNumber = start;
        _endScriptNumber = end;

        ScriptInfo tempScriptInfo = _scriptInfo[_nowScriptNumber];

        StartCoroutine(TypingText(tempScriptInfo._script, _typingSpeed, tempScriptInfo._onPortriat, (int)tempScriptInfo._characterIndex, tempScriptInfo._onEnd));
    }

    IEnumerator TypingText(string message, float speed, bool portrait, int characterIndex, bool endSignal)
    {
        if (endSignal)
            _onEnd = endSignal;

        _arrowImage.enabled = false;

        if (characterIndex >= 0) _name.text = _characterInfo[characterIndex]._name;
        else _name.text = "";

        if (_nowCharacterIndex != characterIndex)
        {
            _portraitImageColorAlphaValue = 0f;
            _portraitImage.color = new Color(1, 1, 1, 0);

            StartCoroutine(FadePortrait());
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
       
        for (int i = 0; i < message.Length; i++)
        {
            if (!audioChannel.isPlaying)
            {
                audioChannel.clip = _messageClip;
                audioChannel.Play();
            }

            _typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }

        audioChannel.Stop();
        _arrowImage.enabled = true;
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
}
