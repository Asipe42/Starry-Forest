using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] FloorGenerator theFloorGenerator;
    [SerializeField] FloorTemplate[] floorSet;

    int level = 0;
    float scrollSpeed;

    void Start()
    {
        SetCandidate(level);
    }

    void Update()
    {
        LevelUp();
    }

    void LevelUp()
    {
        // TODO: Set level up condition;
        if (false)
        {
            level++;
            SetCandidate(level);
        }
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
