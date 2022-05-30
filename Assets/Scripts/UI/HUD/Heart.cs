using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Heart : MonoBehaviour
{
    enum FillState
    {
        Recover,
        Damaged
    }

    [SerializeField] Image[] heartImages;
    [SerializeField] float duration;

    int maxHp;
    int beforeHp;

    void Awake()
    {
        Initialize();
    }

    #region Initial Setting
    void Initialize()
    {
        Status status = PlayerController.instance.theStatus;

        maxHp = status.maxHp;
        beforeHp = status.currentHp;

        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].fillAmount = 1;
        }
    }
    #endregion

    /// <summary>
    /// currentHP에 맞게 Heart UI를 조정한다.
    /// </summary>
    /// <param name="currentHp"></param>
    public void CheckHp(int currentHp)
    {
        if (currentHp < 0)
            return;

        if (currentHp > maxHp)
            return;

        if (beforeHp < currentHp)
        {
            FillHeart(currentHp, this.duration, FillState.Recover);
        } 
        else
        {
            FillHeart(currentHp, this.duration, FillState.Damaged);
        }

        beforeHp = currentHp;
    }

    void FillHeart(int index, float duration, FillState state)
    {
        if (state == FillState.Recover)
        {
            heartImages[index - 1].fillAmount = 0f;
            heartImages[index - 1].DOKill();
            heartImages[index - 1].DOFillAmount(1f, duration);
        }
        else
        {
            heartImages[index].fillAmount = 1f;
            heartImages[index].DOKill();
            heartImages[index].DOFillAmount(0f, duration);
        }
    }
}
