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

    public void LevelUp()
    {
        level++;
        SetCandidate(level);
    }

    public void SetCandidate(int index)
    {
        if (index < 0 && index > floorSet.Length)
        {
            Debug.LogError("out of range");
            return;
        }

        theFloorGenerator.candidate = floorSet[index].floor;
    }
}
