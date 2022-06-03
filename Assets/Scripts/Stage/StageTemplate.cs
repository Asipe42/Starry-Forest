using UnityEngine;

[CreateAssetMenu(fileName = "New Stage", menuName = "Stage/Stage Template")]
public class StageTemplate : ScriptableObject
{
    public int currentStageIndex;
    public int levelUpTimeRate;
    public string currentSceneName;
    public GameObject lastFloor;
}
