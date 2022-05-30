using UnityEngine;

[CreateAssetMenu(fileName = "New Stage Button", menuName = "Stage/Stage Button Template")]
public class StageButtonTemplate : ScriptableObject
{
    public string chapterName;
    public string stageName;
    public string sceneName;
    public Grade clearGrade;
}