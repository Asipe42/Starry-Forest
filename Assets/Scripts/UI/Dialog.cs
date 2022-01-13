using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public int _characterIndex;
    public bool _onPortriat;
}

public class Dialog : MonoBehaviour
{
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
    
    [Header("Texts")]
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] Text _typingText;
    [SerializeField] float _typingSpeed;
    [SerializeField] AudioClip _messageClip;

    [Header("etc")]
    [SerializeField] Image _arrowImage;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        NextScript();
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

                StartCoroutine(TypingText(tempScriptInfo._script, _typingSpeed, tempScriptInfo._onPortriat, tempScriptInfo._characterIndex));                  
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

        StartCoroutine(TypingText(tempScriptInfo._script, _typingSpeed, tempScriptInfo._onPortriat, tempScriptInfo._characterIndex));
    }

    IEnumerator TypingText(string message, float speed, bool portrait, int characterIndex)
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

        for (int i = 0; i < message.Length; i++)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = _messageClip;
                audioSource.Play();
            }

            _typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }

        audioSource.Stop();
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
