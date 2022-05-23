using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageManager : MonoBehaviour
{
    enum DirectionType
    {
        Right,
        Left
    }

    [SerializeField] StageButton[] stages;
    [SerializeField] TextMeshProUGUI chpater;
    [SerializeField] TextMeshProUGUI stage;
    [SerializeField] Image rankBox;
    [SerializeField] Image rank;
    [SerializeField] Sprite[] rankSprites;
    [SerializeField] GameObject offset;

    StageButton currentStageButton;

    bool onRight, onLeft, onSelect;

    void Update()
    {
        InputKey();
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
            Select();
        }
    }

    void ChangeStage(DirectionType directionType)
    {

    }

    void Select()
    {
        
    }

    void SetStageName()
    {
        //this.chpater.text;
        //this.stage.text
    }

    void SetStageInfo(string chapter, string stage, Grade grade)
    {
        this.chpater.text = chapter;
        this.stage.text = stage;

        switch (grade)
        {
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
}
