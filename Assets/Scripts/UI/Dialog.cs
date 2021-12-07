using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialog : MonoBehaviour
{
    [Header("Dialog")]
    [SerializeField] Image _portrait;
    [SerializeField] Image _arrow;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] Text _typingText;
    [SerializeField] float _typingSpeed;
    [SerializeField] AudioClip _messageClip;
    [SerializeField, TextArea] string[] _script;
    [SerializeField] int[] _portraitIndex;
    [SerializeField] string[] _names;

    [SerializeField] bool[] _condition = new bool[10];
    int _nowScriptNumber = 0;
    int _endScriptNumber = 0;
    bool _onEnd;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    private void Update()
    {
        NextScript();
    }

    public void SetCondition(int index)
    {
        if (!_condition[index])
        {
            _condition[index] = true;
            Checkcondition();
        }
    }

    public void Checkcondition()
    {
        if (_condition[0])
        {
            PrintScript(0, 2);
            _onEnd = true;
            _condition[0] = false;
        }
    }

    private void NextScript()
    {
        if (Input.GetButtonDown("Submit") && _arrow.enabled)
        {
            if (_nowScriptNumber < _endScriptNumber)
            {
                _nowScriptNumber++;
                StartCoroutine(TypingText(_script[_nowScriptNumber], _typingSpeed, _portraitIndex[_nowScriptNumber], _names[_nowScriptNumber]));
            }
            else if (_onEnd)
            {
                LoadingSceneController.LoadScene("End");
                GameManager.instance.UIManagerInstance.SetResult(true);
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

        StartCoroutine(TypingText(_script[_nowScriptNumber], _typingSpeed, _portraitIndex[_nowScriptNumber], _names[_nowScriptNumber]));
    }

    IEnumerator TypingText(string message, float speed, int portrait, string name)
    {
        _arrow.enabled = false;
        _name.text = name;

        switch (portrait)
        {
            case 0:
                _portrait.enabled = true;
                break;
            case 1:
                _portrait.enabled = false;
                break;
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
        _arrow.enabled = true;
    }
}
