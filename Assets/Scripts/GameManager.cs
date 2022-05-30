using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool[] isLockedStage;
    public Grade[] highGrade;
    public int life = 3;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public void UnlockStage(int index, bool state = false)
    {
        isLockedStage[index] = state;
    }

    public void RecordGrade(int index, Grade grade)
    {
        highGrade[index] = grade;
    }
}
