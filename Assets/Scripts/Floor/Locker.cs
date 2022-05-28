using UnityEngine;

public class Locker : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InputManager.instance.onLock = true;
        }
    }
}