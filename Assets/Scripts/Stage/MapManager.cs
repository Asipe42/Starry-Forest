using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class MapManager : MonoBehaviour
{
    enum DirectionType
    {
        Right,
        Left
    }

    [SerializeField] TextMeshProUGUI chapterText;
    [SerializeField] TextMeshProUGUI stageText;
    [SerializeField] TextMeshProUGUI lifeText;
    [SerializeField] Image rankBox;
    [SerializeField] Image rank;
    [SerializeField] Sprite[] rankSprites;
    [SerializeField] GameObject offset;
    [SerializeField] GameObject emotion;

    public StageButton[] stages;

    Stage stage;
    StageButton currentStageButton;
    FadeScreen fadeScreen;

    int currnetStageIndex = 0;
    int maxStageIndex = 10;
    bool onRight, onLeft, onSelect;
    bool onLoading;
    bool onLock;
    bool endFade;

    AudioClip notificationClip;

    void Awake()
    {
        Initialize();
        LockCursor();
        GetAudioClip();
        SubscribeEvent();
        SetOffset();
    }

    #region Initial Setting
    void Initialize()
    {
        stage = GameObject.FindObjectOfType<Stage>();
        fadeScreen = GameObject.FindObjectOfType<FadeScreen>();
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void GetAudioClip()
    {
        notificationClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Notification");
    }

    void SubscribeEvent()
    {
        FadeScreen.fadeEvent -= EndFade;
        FadeScreen.fadeEvent += EndFade;
    }
    #endregion

    void EndFade(bool state)
    {
        endFade = state;
    }

    void SetOffset()
    {
        currnetStageIndex = GameManager.lastSelectedStageButtonIndex;
        currentStageButton = stages[GameManager.lastSelectedStageButtonIndex];
        MoveToStageButton(GameManager.lastSelectedStageButtonIndex, 0.01f);
    }

    void GetCurrentStageButton()
    {
        currentStageButton = offset.transform.parent.GetComponent<StageButton>();
    }

    void Start()
    {
        StartCoroutine(EnableStage());
    }

    void Update()
    {
        if (!onLock)
        {
            InputKey();
        }
    }

    void InputKey()
    {
        onRight = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
        onLeft = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
        onSelect = Input.GetButtonDown("Submit");

        if (onRight && !onLeft)
        {
            ChangeStage(DirectionType.Right);
        }
        else if (!onRight && onLeft)
        {
            ChangeStage(DirectionType.Left);
        }

        if (onSelect)
        {
            if (!onLoading)
            {
                onLoading = true;
                StartCoroutine(Select());
            }
        }
    }

    void ChangeStage(DirectionType directionType)
    {
        if (directionType == DirectionType.Left)
        {
            if (currnetStageIndex < 1)
                return;

            if (!stages[currnetStageIndex - 1].onLock)
            {
                currnetStageIndex--;
                ChangeStateLogic();
            }
        }

        if (directionType == DirectionType.Right)
        {
            if (currnetStageIndex > maxStageIndex - 1)
                return;

            if (!stages[currnetStageIndex + 1].onLock)
            {
                currnetStageIndex++;
                ChangeStateLogic();
            }
        }
    }

    void ChangeStateLogic()
    {
        DisableStage();
        MoveToStageButton(currnetStageIndex);
        GetCurrentStageButton();
    }

    void MoveToStageButton(int index, float duration = 0.75f)
    {
        Vector3 destination = new Vector3(offset.transform.parent.transform.position.x,
                                          offset.transform.parent.transform.position.y + 0.6f,
                                          offset.transform.parent.transform.position.z);

        offset.transform.parent = stages[index].transform;
        offset.transform.DOMove(destination, duration).OnComplete(() => StartCoroutine(EnableStage()));
    }

    IEnumerator Select()
    {
        DisableStage();
        GameManager.lastSelectedStageButtonIndex = currnetStageIndex;
        fadeScreen.FadeScreenEffect(1f, 0.5f, 1f);

        SFXController.instance.PlaySFX(notificationClip);
        BGMController.instance.FadeVolume(0f, 1.75f);
        emotion.transform.DOScale(0.5f, 0.5f).SetEase(Ease.OutBounce);

        yield return new WaitForSeconds(2f);
        Loading.LoadScene(currentStageButton.stageTemplate.sceneName);
    }

    void SetStageInfo(StageButtonTemplate stageTemplate)
    {
        this.chapterText.text = stageTemplate.chapterName;
        this.stageText.text = stageTemplate.stageName;
        this.lifeText.text = "" + GameManager.life;

        switch (stageTemplate.clearGrade)
        {
            case Grade.None:
                break;
            case Grade.APlus:
                rank.sprite = rankSprites[0];
                break;
            case Grade.A:
                rank.sprite = rankSprites[1];
                break;
            case Grade.BPlus:
                rank.sprite = rankSprites[2];
                break;
            case Grade.B:
                rank.sprite = rankSprites[3];
                break;
            case Grade.CPlus:
                rank.sprite = rankSprites[4];
                break;
            case Grade.C:
                rank.sprite = rankSprites[5];
                break;
        }
    }

    IEnumerator EnableStage()
    {
        onLock = true;
        yield return new WaitUntil(() => endFade);

        stage.ShowStageInfo(0.5f);
        stage.ShowStartGuide(0.5f);
        stage.ShowAlbumGuide(0.5f);
        SetStageInfo(currentStageButton.stageTemplate);

        yield return new WaitForSeconds(0.5f);
        onLock = false;
    }

    void DisableStage()
    {
        onLock = true;

        stage.HideStageInfo(0.3f);
        stage.HideStartGuide(0.3f);
        stage.HideAlbumGuide(0.3f);
    }
}
