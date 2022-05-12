using UnityEngine;

public class MushroomManager : MonoBehaviour
{
    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Mushroom>().delay = (float)i * 0.25f;
        }
    }
}
