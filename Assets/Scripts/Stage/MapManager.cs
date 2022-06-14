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
    [SerializeField] Transform[] chapterTransform;
    [SerializeField] int[] chapterIndex;

    public StageButton[] stages;

    Stage stage;
    StageButton currentStageButton;
    FadeScreen fadeScreen;
    Animator anim;

    int currentStageIndex = 0;
    int maxStageIndex = 10;
    bool onRight, onLeft, onSelect;
    bool onLoading;
    bool onLock;
    bool endFade;
    bool checkChapter;
    bool changedChapter;
    bool onChange;

    AudioClip notificationClip;

    void Awake()
    {
        Initialize();
        GetAudioClip();
        SubscribeEvent();
        SetOffset();
        CheckLife();
    }

    #region Initial Setting
    void Initialize()
    {
        stage = GameObject.FindObjectOfType<Stage>();
        fadeScreen = GameObject.FindObjectOfType<FadeScreen>();
        anim = offset.GetComponent<Animator>();
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

    void CheckLife()
    {
        if (GameManager.life <= 0)
        {
            Loading.LoadScene("Title");
            GameManager.InitializeStageButtonInfo();
            GameManager.InitializeLifeCount();
        }
    }
    #endregion

    void EndFade(bool state)
    {
        endFade = state;
    }

    void SetOffset()
    {
        currentStageIndex = GameManager.lastSelectedStageButtonIndex;
        currentStageButton = stages[GameManager.lastSelectedStageButtonIndex];
        Camera.main.transform.position = new Vector3(chapterTransform[GameManager.currentChapterIndex].position.x, chapterTransform[GameManager.currentChapterIndex].position.y, -10);
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
            if (onChange)
                return;

            onChange = true;
            ChangeStage(DirectionType.Right);
        }
        else if (!onRight && onLeft)
        {
            if (onChange)
                return;

            onChange = true;
            ChangeStage(DirectionType.Left);
        }

        if (onSelect)
        {
            if (!onLoading)
            {
                onLoading = true;
                StartCoroutine(SelectLogic());
            }
        }
    }

    void ChangeStage(DirectionType directionType)
    {
        bool checkChapter = false;
        changedChapter = false;

        if (directionType == DirectionType.Left)
        {
            if (currentStageIndex < 1)
                return;

            for (int i = 0; i < chapterIndex.Length; i++)
            {
                if (currentStageIndex == chapterIndex[i])
                {
                    checkChapter = false;
                    GameManager.currentChapterIndex = i - 1;
                    StartCoroutine(ChangeChapter(i - 1));
                    break;
                }
                else
                {
                    checkChapter = true;
                }
            }

            if (checkChapter)
                changedChapter = true;

            if (!stages[currentStageIndex - 1].onLock)
            {
                currentStageIndex--;
                offset.transform.rotation = Quaternion.Euler(0, 180f, 0f);
                StartCoroutine(ChangeStateLogic());
            }
        }

        if (directionType == DirectionType.Right)
        {
            if (currentStageIndex > maxStageIndex - 1)
                return;

            for (int i = 0; i < chapterIndex.Length; i++)
            {
                if (i == GameManager.currentChapterIndex)
                    continue;

                if (currentStageIndex + 1 == chapterIndex[i])
                {
                    checkChapter = false;
                    GameManager.currentChapterIndex = i;
                    StartCoroutine(ChangeChapter(i));
                    break;
                }
                else
                {
                    checkChapter = true;
                }
            }

            if (checkChapter)
                changedChapter = true;

            if (!stages[currentStageIndex + 1].onLock)
            {
                currentStageIndex++;
                offset.transform.rotation = Quaternion.Euler(0, 0f, 0f);
                StartCoroutine(ChangeStateLogic());
            }
        }
    }

    IEnumerator ChangeChapter(int index)
    {
        fadeScreen.FadeScreenEffect(1f, 0.25f);
        yield return new WaitForSeconds(0.25f);
        changedChapter = true;
        Camera.main.transform.DOMove(new Vector3(chapterTransform[index].position.x, chapterTransform[index].position.y, -10), 0.5f)
                             .OnComplete(() => 
                             {
                                 fadeScreen.FadeScreenEffect(0f, 0.5f);
                             });
    }

    IEnumerator ChangeStateLogic()
    {
        yield return new WaitUntil(() => changedChapter);
        DisableStage();
        MoveToStageButton(currentStageIndex);
        GetCurrentStageButton();
        changedChapter = false;
    }

    void MoveToStageButton(int index, float duration = 0.75f)
    {
        anim.SetBool("move", true);
        offset.transform.parent = stages[index].transform;

        Vector3 destination = new Vector3(offset.transform.parent.transform.position.x,
                                          offset.transform.parent.transform.position.y + 0.6f,
                                          offset.transform.parent.transform.position.z);

        offset.transform.DOMove(destination, duration)
                        .OnComplete(() => 
                        { 
                            StartCoroutine(EnableStage());
                            anim.SetBool("move", false);
                            offset.transform.DORotate(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.OutQuad);
                            onChange = false;
                        });
    }

    public void Select()
    {
        if (!onLoading)
        {
            onLoading = true;
            StartCoroutine(SelectLogic());
        }
    }

    IEnumerator SelectLogic()
    {
        DisableStage();
        GameManager.lastSelectedStageButtonIndex = currentStageIndex;
        fadeScreen.FadeScreenEffect(1f, 0.5f, 1f);

        SFXController.instance.PlaySFX(notificationClip);
        BGMController.instance.FadeVolume(0f, 1.75f);
        emotion.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);

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
