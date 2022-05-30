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
        stage = GameObject.FindObjectOfType<Stage>();
        fadeScreen = GameObject.FindObjectOfType<FadeScreen>();

        notificationClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Notification");

        FadeScreen.FadeEvent -= EndFade;
        FadeScreen.FadeEvent += EndFade;

        GetCurrentStageButton();
    }

    void Start()
    {
        StartCoroutine(EnableStage());
    }

    void GetCurrentStageButton()
    {
        currentStageButton = offset.transform.parent.GetComponent<StageButton>();
    }

    void EndFade(bool state)
    {
        endFade = state;
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
                DisableStage();
                currnetStageIndex--;
                MoveToStageButton(currnetStageIndex);
                GetCurrentStageButton();
                SetStageInfo(currentStageButton.stageTemplate);
            }
        }

        if (directionType == DirectionType.Right)
        {
            if (currnetStageIndex > maxStageIndex - 1)
                return;

            if (!stages[currnetStageIndex + 1].onLock)
            {
                DisableStage();
                currnetStageIndex++;
                MoveToStageButton(currnetStageIndex);
                GetCurrentStageButton();
                SetStageInfo(currentStageButton.stageTemplate);
            }
        }
    }

    void MoveToStageButton(int index)
    {
        offset.transform.parent = stages[index].transform;
        offset.transform.DOMove(offset.transform.parent.transform.position, 0.5f).OnComplete(() => StartCoroutine(EnableStage()));
    }

    IEnumerator Select()
    {
        DisableStage();
        fadeScreen.FadeScreenEffect(1f, 0.5f, 1f);

        SFXController.instance.PlaySFX(notificationClip);
        emotion.transform.DOScale(0.5f, 0.5f).SetEase(Ease.OutBounce);

        yield return new WaitForSeconds(2f);
        Loading.LoadScene(currentStageButton.stageTemplate.sceneName);
    }

    void SetStageInfo(StageButtonTemplate stageTemplate)
    {
        this.chapterText.text = stageTemplate.chapterName;
        this.stageText.text = stageTemplate.stageName;
        this.lifeText.text = "" + GameManager.instance.life;

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

        stage.ShowStageInfo(0.25f);
        stage.ShowStartGuide(0.25f);
        stage.ShowAlbumGuide(0.25f);
        SetStageInfo(currentStageButton.stageTemplate);

        yield return new WaitForSeconds(1f);
        onLock = false;
    }

    void DisableStage()
    {
        onLock = true;
        stage.HideStageInfo(0.25f);
        stage.HideStartGuide(0.25f);
        stage.HideAlbumGuide(0.25f);
    }
}
