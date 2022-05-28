using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool[] isLockedStage;

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
}
