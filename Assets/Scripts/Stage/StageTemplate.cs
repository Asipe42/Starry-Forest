using UnityEngine;

[CreateAssetMenu(fileName = "New Stage", menuName = "Stage/Stage Template")]
public class StageTemplate : ScriptableObject
{
    public string chapterName;
    public string stageName;
    public string sceneName;
    public Grade clearGrade;
}