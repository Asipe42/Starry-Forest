using UnityEngine;

public class StageButton : MonoBehaviour
{
    [SerializeField] Sprite[] sprite; // 0: Unlock, 1: lock
    [SerializeField] int index;

    public StageButtonTemplate stageTemplate;

    public bool onLock;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        Initialize();
        ChangeColor(onLock);
    }

    #region Initial Setting
    void Initialize()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        onLock = GameManager.stageButtonInfo[index].isLockedStage;

        if (stageTemplate.clearGrade == Grade.None)
            stageTemplate.clearGrade = GameManager.stageButtonInfo[index].highGrade;

        if (stageTemplate.clearGrade > GameManager.stageButtonInfo[index].highGrade)
            stageTemplate.clearGrade = GameManager.stageButtonInfo[index].highGrade;
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
    #endregion
}