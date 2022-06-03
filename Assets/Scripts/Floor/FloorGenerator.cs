using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    [SerializeField] Transform floorGroup;
    [SerializeField] Vector3 createPosition = new Vector3(153.6f, -0.8f, 0f);

    public GameObject[] candidate;

    /// <summary>
    /// position에 floor을 생성한다. (last가 true라면 마지막 floor)
    /// </summary>
    /// <param name="position"></param>
    /// <param name="last"></param>
    /// <returns></returns>
    public GameObject CreateFloor(Vector3 position, bool last = false)
    {
        if (last)
        {
            return Instantiate(StageManager.instance.stageTemplate.lastFloor, position + createPosition, Quaternion.identity, floorGroup);
        }
        else
        {
            int index = Random.Range(0, candidate.Length);
            return Instantiate(candidate[index], position + createPosition, Quaternion.identity, floorGroup);
        }
    }

    /// <summary>
    /// floor를 제거한다
    /// </summary>
    /// <param name="floor"></param>
    public void DestroyFloor(GameObject floor)
    {
        Destroy(floor);
    }
}
