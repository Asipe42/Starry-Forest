using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    [SerializeField] Transform floorGroup;
    [SerializeField] Vector3 createPosition = new Vector3(153.6f, -0.8f, 0f);

    public GameObject[] candidate;

    public GameObject CreateFloor(Vector3 position, bool last = false)
    {
        if (last)
        {
            return Instantiate(FloorManager.instance.stageTemplate.lastFloor, position + createPosition, Quaternion.identity, floorGroup);
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
