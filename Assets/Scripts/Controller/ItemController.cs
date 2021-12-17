using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    Mushroom,
    Potion,
    Dandelion
}

public class ItemController : MonoBehaviour
{
    [SerializeField] protected ItemType _myType = ItemType.None;
}
