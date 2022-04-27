using UnityEngine;

public class LastFloor : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().ReachLastFloor = true;
        }
    }
}
