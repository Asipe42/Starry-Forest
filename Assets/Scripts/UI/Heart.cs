using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum FillState
{
    Up,
    Down
}

public class Heart : MonoBehaviour
{
    [SerializeField] Image[] heartImages;
    [SerializeField] float duration;

    Status theStatus;

    int hp;

    void Start()
    {
        theStatus = GameObject.FindObjectOfType<Status>().GetComponent<Status>();
        hp = theStatus.hp;

        Initialize();
    }

    void Initialize()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].fillAmount = 1;
        }
    }

    public void CheckHp(int currentHp)
    {
        if (currentHp < 0)
            return;

        if (currentHp >= theStatus.maxHp)
            return;

        if (hp < currentHp)
        {
            FillHeart(currentHp, this.duration, FillState.Up);
        }
        else
        {
            FillHeart(currentHp, this.duration, FillState.Down);
        }

        hp = currentHp;
    }

    void FillHeart(int index, float duration, FillState state)
    {
        if (state == FillState.Up)
        {
            heartImages[index].fillAmount = 0f;
            heartImages[index].DOFillAmount(1f, duration);
        }
        else
        {
            heartImages[index].fillAmount = 1f;
            heartImages[index].DOFillAmount(0f, duration);
        }
    }
}
