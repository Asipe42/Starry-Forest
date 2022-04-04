using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FloorManager : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] FloorGenerator theFloorGenerator;
    [SerializeField] FloorTemplate[] floorSet;

    public int level = 0;

    void Start()
    {
        SetCandidate(level);
    }

    public void LevelUp()
    {
        text.DOColor(Color.red, 2f);
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
