using UnityEngine;

public class StageButton : MonoBehaviour
{
    [SerializeField] Color unlockColor;
    [SerializeField] Color lockColor;
    [SerializeField] int index;

    public StageTemplate stageTemplate;

    public bool onLock;

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        onLock = GameManager.instance.isLockedStage[index];

        ChangeColor(onLock);
    }

    public void ChangeColor(bool state)
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
}
