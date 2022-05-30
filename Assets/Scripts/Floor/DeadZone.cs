using UnityEngine;

public class DeadZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Loading.LoadScene(FloorManager.instance.stageTemplate.currentSceneName);
        }
    }
}
