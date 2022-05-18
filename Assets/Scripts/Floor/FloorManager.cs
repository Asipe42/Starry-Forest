using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] FloorGenerator theFloorGenerator;
    [SerializeField] FloorTemplate[] floorSet;

    public int level = 0;

    void Start()
    {
        SetCandidate(level);
    }

    /// <summary>
    /// ÇÃ·§Æû ³­ÀÌµµ¸¦ ¿Ã¸°´Ù.
    /// </summary>
    public void LevelUp()
    {
        level++;
        SetCandidate(level);
    }

    /// <summary>
    /// ÇÃ·§Æû ±×·ìÀ» ±³Ã¼ÇÑ´Ù.
    /// </summary>
    /// <param name="index"></param>
    public void SetCandidate(int index)
    {
        if (index < 0 || index >= floorSet.Length)
        {
            Debug.LogWarning("out of range");
            return;
        }

        theFloorGenerator.candidate = floorSet[index].floor;
    }
}
