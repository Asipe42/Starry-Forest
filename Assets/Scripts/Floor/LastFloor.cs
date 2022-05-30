using UnityEngine;

public class LastFloor : MonoBehaviour
{
    [SerializeField] LastFloorState state;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(StageManager.instance.FinishStage(state));
        }
    }
}
