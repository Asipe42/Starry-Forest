using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public enum CharacterType
{
    None = 0,
    Dal,
    Daram
}

[System.Serializable]
class CharacterInfo
{
    public Sprite portrait;
    public Vector3 scale;
    public string name;
}

[System.Serializable]
class ScriptInfo
{
    [TextArea] public string script;
    public CharacterType characterType;
    public bool onPortrait;
    public bool onEnd;
}

[System.Serializable]
struct ScriptIndex
{
    public int start;
    public int end;
}

public class Dialog : MonoBehaviour
{
    [SerializeField] CharacterInfo[] characterInfo;
    [SerializeField] ScriptInfo[] scriptInfo;
    [SerializeField] ScriptIndex[] scriptIndex;

    [SerializeField] Image portrait;
    [SerializeField] Image arrow;
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI typingText;
    [SerializeField] float typingSpeed;

    AudioClip messageClip;
    AudioSource audioSource;

    int startScriptNumber;
    int endScriptNumber;
    int presentScriptNumber;
    int scriptIndexNumber;
    int presentCharacterIndex = -1;

    bool onEnd;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        messageClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Message");
    }

    void Update()
    {
        NextScript();
    }

    void Start()
    {
        startScriptNumber = scriptIndex[scriptIndexNumber].start;
        endScriptNumber = scriptIndex[scriptIndexNumber].end;
    }

    public void StartScript()
    {
        PrintScript(startScriptNumber, endScriptNumber);
    }

    void PrintScript(int start, int end)
    {
        presentScriptNumber = start;
        endScriptNumber = end;

        ScriptInfo temp = scriptInfo[presentScriptNumber];

        StartCoroutine(TypingText(temp.script, 0.01f, temp.onPortrait, (int)temp.characterType, temp.onEnd));
    }

    void NextScript()
    {
        if (Input.GetButtonDown("Submit") && arrow.enabled)
        {
            if (presentScriptNumber < endScriptNumber)
            {
                presentScriptNumber++;

                ScriptInfo temp = scriptInfo[presentScriptNumber];
                StartCoroutine(TypingText(temp.script, 0.01f, temp.onPortrait, (int)temp.characterType, temp.onEnd));
            }
            else if (!onEnd)
            {
                scriptIndexNumber++;
                startScriptNumber = scriptIndex[scriptIndexNumber].start;
                endScriptNumber = scriptIndex[scriptIndexNumber].end;

                gameObject.SetActive(false);
                TimelineController.instance.ContinueTimeline();
            }
            else if (onEnd)
            {
                // Load Next Scene
            }
        }
    }

    IEnumerator TypingText(string message, float speed, bool onPortrait, int characterIndex, bool onEnd)
    {
        if (onEnd)
            this.onEnd = onEnd;

        arrow.enabled = false;

        if (characterIndex >= 0) name.text = characterInfo[characterIndex].name;
        else name.text = "";

        if (presentCharacterIndex != characterIndex)
        {
            portrait.color = Color.clear;
            portrait.DOColor(Color.white, 1f);
        }

        presentCharacterIndex = characterIndex;

        if (onPortrait)
        {
            portrait.enabled = true;
            portrait.sprite = characterInfo[characterIndex].portrait;
            portrait.rectTransform.localScale = characterInfo[characterIndex].scale;
        }
        else
        {
            portrait.enabled = false;
        }


        for (int i = 0; i < message.Length; i++)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = messageClip;
                audioSource.Play();
            }

            typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }

        audioSource.Stop();
        arrow.enabled = true;
    }
}
