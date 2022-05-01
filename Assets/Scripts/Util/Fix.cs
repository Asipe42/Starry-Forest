using UnityEngine;

public class Fix : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}