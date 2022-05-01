using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public GameObject[] candidate;

    [SerializeField] Vector3 createPosition = new Vector3(153.6f, -0.8f, 0f);
    [SerializeField] Transform floorGroup;

    public GameObject lastFloor;

    public GameObject CreateFloor(Vector3 position, bool last = false)
    {
        if (last)
        {
            return Instantiate(lastFloor, position + createPosition, Quaternion.identity, floorGroup);
        }
        else
        {
            int index = Random.Range(0, candidate.Length);
            return Instantiate(candidate[index], position + createPosition, Quaternion.identity, floorGroup);
        }
    }

    public void DestroyFloor(GameObject floor)
    {
        Destroy(floor);
    }
}
