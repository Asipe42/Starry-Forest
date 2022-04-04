using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] string preSceneName;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Loading.LoadScene(preSceneName);
        }
    }
}
