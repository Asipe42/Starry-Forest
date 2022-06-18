using UnityEngine;
using DG.Tweening;

public class StageButton : MonoBehaviour
{
    [SerializeField] Sprite[] sprite; // 0: Unlock, 1: lock
    [SerializeField] int index;
    [SerializeField] Sprite[] rankSprites;

    public StageButtonTemplate stageButtonTemplate;

    public bool onLock;
    public bool onRank = true;

    SpriteRenderer spriteRenderer;
    GameObject rank;

    AudioClip showRankClip;

    void Start()
    {
        Initialize();
        GetAudioClip();
        ChangeColor(onLock);

        if (onRank)
        {
            SetRank(stageButtonTemplate);
            PlayRankAnimation();
        }

    }

    #region Initial Setting
    void Initialize()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (onRank)
            rank = transform.GetChild(0).transform.GetChild(0).gameObject;

        onLock = GameManager.stageButtonInfo[index].isLockedStage;
        stageButtonTemplate.clearGrade = GameManager.stageButtonInfo[index].highGrade;
    }

    void GetAudioClip()
    {
        showRankClip = Resources.Load<AudioClip>("Audio/SFX/SFX_ShowRank");
    }

    void ChangeColor(bool state)
    {
        if (state) // lock
        {
            spriteRenderer.sprite = sprite[1];
        }
        else // unlcok
        {
            spriteRenderer.sprite = sprite[0];
        }
    }

    void SetRank(StageButtonTemplate myTemplate)
    {
        SpriteRenderer rankSR = rank.GetComponent<SpriteRenderer>();

        switch (myTemplate.clearGrade)
        {
            case Grade.None:
                rankSR.sprite = null;
                break;
            case Grade.APlus:
                rankSR.sprite = rankSprites[0];
                break;
            case Grade.A:
                rankSR.sprite = rankSprites[1];
                break;
            case Grade.BPlus:
                rankSR.sprite = rankSprites[2];
                break;
            case Grade.B:
                rankSR.sprite = rankSprites[3];
                break;
            case Grade.CPlus:
                rankSR.sprite = rankSprites[4];
                break;
            case Grade.C:
                rankSR.sprite = rankSprites[5];
                break;
        }
    }

    void PlayRankAnimation()
    {
        if (stageButtonTemplate.clearGrade != Grade.None)
        {
            float rankScale = rank.transform.localScale.x;

            var sequence = DOTween.Sequence();

            sequence.Append(rank.transform.parent.transform.DOScale(1f, 1f).SetEase(Ease.OutBounce).OnStart(() => SFXController.instance.PlaySFX(showRankClip)))
                    .Append(rank.transform.DOScale(rankScale + 0.1f, 2f).SetEase(Ease.InOutCirc).SetLoops(-1, LoopType.Yoyo));
        }
    }
    #endregion
}