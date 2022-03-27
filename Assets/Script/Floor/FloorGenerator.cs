using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public GameObject[] candidate;

    [SerializeField] Vector3 createPosition = new Vector3(115.2f, 0f, 0f);
    [SerializeField] Transform floorGroup;

    public GameObject CreateFloor(Vector3 position)
    {
        int _index = Random.Range(0, candidate.Length);
        return Instantiate(candidate[_index], position + createPosition, Quaternion.identity, floorGroup);
    }

    public void DestroyFloor(GameObject floor)
    {
        Destroy(floor);
    }
}
