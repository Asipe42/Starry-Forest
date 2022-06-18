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
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject albumButton;
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
    bool changedChapter;
    bool onChange;

    AudioClip notificationClip;
    AudioClip errorClip;

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
        errorClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Error");
    }

    void SubscribeEvent()
    {
        FadeScreen.fadeEvent -= EndFade;
        FadeScreen.fadeEvent += EndFade;
    }

    void SetOffset()
    {
        currentStageIndex = GameManager.lastSelectedStageButtonIndex;
        currentStageButton = stages[GameManager.lastSelectedStageButtonIndex];
        Camera.main.transform.position = new Vector3(chapterTransform[GameManager.currentChapterIndex].position.x, chapterTransform[GameManager.currentChapterIndex].position.y, -10);
        MoveToStageButton(GameManager.lastSelectedStageButtonIndex, 0.01f);
    }

    void CheckLife()
    {
        if (GameManager.life <= 0)
        {
            //TO-DO: GameOver Directing
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

        if (!onChange)
        {
            if (onRight && !onLeft)
            {
                ChangeStage(DirectionType.Right);
            }
            else if (!onRight && onLeft)
            {
                ChangeStage(DirectionType.Left);
            }
        }

        if (!onLoading)
        {
            if (onSelect)
            {
                StartCoroutine(SelectLogic());
            }
        }
    }

    void ChangeStage(DirectionType directionType)
    {
        onChange = true;
        changedChapter = false;

        if (directionType == DirectionType.Left)
        {
            if (currentStageIndex < 1)
            {
                onChange = false;
                return;
            }

            CheckChapterIndex(directionType);
            CheckAvailableStageButton(directionType);
        }

        if (directionType == DirectionType.Right)
        {
            if (currentStageIndex >= maxStageIndex - 1)
            {
                onChange = false;
                return;
            }

            CheckChapterIndex(directionType);
            CheckAvailableStageButton(directionType);
        }
    }

    void CheckChapterIndex(DirectionType directionType)
    {
        changedChapter = true;

        if (directionType == DirectionType.Left)
        {
            for (int i = 0; i < chapterIndex.Length; i++)
            {
                if (currentStageIndex == chapterIndex[i])
                {
                    changedChapter = false;
                    GameManager.currentChapterIndex = i - 1;
                    StartCoroutine(ChangeChapter(i - 1));
                    break;
                }
            }
        }

        if (directionType == DirectionType.Right)
        {
            for (int i = 0; i < chapterIndex.Length; i++)
            {
                if (i == GameManager.currentChapterIndex)
                    continue;

                if (currentStageIndex + 1 == chapterIndex[i])
                {
                    changedChapter = false;
                    GameManager.currentChapterIndex = i;
                    StartCoroutine(ChangeChapter(i));
                    break;
                }
            }
        }
    }

    void CheckAvailableStageButton(DirectionType directionType)
    {
        float angleValue = 0f;

        if (directionType == DirectionType.Left)
        {
            angleValue = 180f;

            if (!stages[currentStageIndex - 1].onLock)
            {
                currentStageIndex--;
                offset.transform.rotation = Quaternion.Euler(0, angleValue, 0f);
                StartCoroutine(ChangeStateLogic());
            }
        }

        if (directionType == DirectionType.Right)
        {
            angleValue = 0f;

            if (!stages[currentStageIndex + 1].onLock)
            {
                currentStageIndex++;
                offset.transform.rotation = Quaternion.Euler(0, angleValue, 0f);
                StartCoroutine(ChangeStateLogic());
            }
        }
    }

    IEnumerator ChangeChapter(int index)
    {
        Vector3 cameraPosition = chapterTransform[index].position;
        cameraPosition.z = -10;
        float duration = 0.3f;

        fadeScreen.FadeScreenEffect(1f, duration);
        yield return new WaitForSeconds(duration);

        changedChapter = true;
        Camera.main.transform.DOMove(cameraPosition, duration * 2f)
                             .OnComplete(() => fadeScreen.FadeScreenEffect(0f, duration * 2f));
    }

    IEnumerator ChangeStateLogic()
    {
        yield return new WaitUntil(() => changedChapter);

        DisableStage();
        MoveToStageButton(currentStageIndex);
        GetCurrentStageButton();

        changedChapter = false;
    }

    void MoveToStageButton(int index, float duration = 0.5f)
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
                            offset.transform.DORotate(new Vector3(0f, 0f, 0f), 0.3f).SetEase(Ease.OutQuad);
                            onChange = false;
                        });
    }

    /// <summary>
    /// [시작] 버튼 전용 함수
    /// </summary>
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
        onLoading = true;

        DisableStage();
        GameManager.lastSelectedStageButtonIndex = currentStageIndex;
        fadeScreen.FadeScreenEffect(1f, 0.5f, 1f);

        SFXController.instance.PlaySFX(notificationClip);
        BGMController.instance.FadeVolume(0f, 1.75f);
        emotion.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);

        yield return new WaitForSeconds(2f);
        Loading.LoadScene(currentStageButton.stageButtonTemplate.sceneName);
    }

    void SetStageInfo(StageButtonTemplate stageTemplate)
    {
        this.chapterText.text = stageTemplate.chapterName;
        this.stageText.text = stageTemplate.stageName;
        this.lifeText.text = "" + GameManager.life;
    }

    IEnumerator EnableStage()
    {
        onLock = true;
        yield return new WaitUntil(() => endFade);

        StartCoroutine(stage.ShowEveryElements(0f, 0.3f));
        SetStageInfo(currentStageButton.stageButtonTemplate);

        yield return new WaitForSeconds(0.3f);
        onLock = false;
    }

    void DisableStage()
    {
        onLock = true;

        StartCoroutine(stage.HideEveryElements(0f, 0.25f));
    }

    public void PlayDisabledAnimation()
    {
        albumButton.transform.DOShakePosition(0.5f, new Vector3(15f, 0f, 0f), randomness: 0f).SetEase(Ease.OutQuad);
        SFXController.instance.PlaySFX(errorClip);
    }
}
