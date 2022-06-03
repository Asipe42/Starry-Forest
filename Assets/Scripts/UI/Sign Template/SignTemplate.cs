using UnityEngine;

[CreateAssetMenu(fileName = "New Sign", menuName = "UI/Sign Template")]
public class SignTemplate : ScriptableObject
{
    public string message;
    public int fontSize;
}
