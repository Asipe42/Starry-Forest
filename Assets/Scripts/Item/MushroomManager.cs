using UnityEngine;

public class MushroomManager : MonoBehaviour
{
    int itemCount;

    void Awake()
    {
        itemCount = transform.childCount;

        for (int i = 0; i < itemCount; i++)
        {
            transform.GetChild(i).GetComponent<Mushroom>().delay = (float)i * 0.25f;
        }
    }

    void Start()
    {
        FloorManager.instance.totalItemCount += itemCount;
    }
}
