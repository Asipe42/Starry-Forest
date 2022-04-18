using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fix : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
