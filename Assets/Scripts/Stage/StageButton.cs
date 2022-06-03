using UnityEngine;

public class StageButton : MonoBehaviour
{
    [SerializeField] Color unlockColor;
    [SerializeField] Color lockColor;
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

        if (stageTemplate.clearGrade > GameManager.stageButtonInfo[index].highGrade)
            stageTemplate.clearGrade = GameManager.stageButtonInfo[index].highGrade;
    }

    void ChangeColor(bool state)
    {
        if (state) // unlock
        {
            spriteRenderer.color = lockColor;
        }
        else // lock
        {
            spriteRenderer.color = unlockColor;
        }
    }
    #endregion
}