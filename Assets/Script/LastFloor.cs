using UnityEngine;

public class LastFloor : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //TO-DO: Excute End Event
        }
    }
}
