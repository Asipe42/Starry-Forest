using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    [SerializeField] int itemCount;

    void Start()
    {
        AddItemCount(itemCount);
    }

    void AddItemCount(int value) => GameManager.instance.StageManagerInstance.totalItemCount += value;
}
