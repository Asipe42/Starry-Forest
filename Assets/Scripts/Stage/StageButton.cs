using UnityEngine;

public class StageButton : MonoBehaviour
{
    [SerializeField] Color unlockColor;
    [SerializeField] Color lockColor;

    public bool onLock;

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
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
