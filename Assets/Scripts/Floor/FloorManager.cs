using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public static FloorManager instance;

    public FloorGenerator floorGenerator;

    [SerializeField] FloorTemplate[] floorSet;

    public int level = 0; // 0 == easy, 1 == hard
    public int totalItemCount;
    public bool gaugeIsFull;

    void Awake()
    {
        Initialize();
        SubscribeEvent();
        SetCandidate(level);
    }

    #region Initial Setting
    void Initialize()
    {
        instance = this;
    }

    void SubscribeEvent()
    {
        ProgressBar.levelUpEvent -= LevelUp;
        ProgressBar.levelUpEvent += LevelUp;

        ProgressBar.fullGaugeEvent -= GaugeFull;
        ProgressBar.fullGaugeEvent += GaugeFull;
    }

    void LevelUp()
    {
        level++;
        SetCandidate(level);
    }

    void GaugeFull(bool state)
    {
        gaugeIsFull = state;
    }
    #endregion

    /// <summary>
    /// ÇÃ·§Æû ±×·ìÀ» ±³Ã¼ÇÑ´Ù.
    /// </summary>
    /// <param name="index"></param>
    public void SetCandidate(int index)
    {
        if (index < 0 || index >= floorSet.Length)
            return;

        floorGenerator.candidate = floorSet[index].floor;
    }
}