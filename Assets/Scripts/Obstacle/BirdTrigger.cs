using UnityEngine;

public class BirdTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Bird bird = transform.parent.GetComponent<Bird>();

            bird.Appear(); bird.onAppear = true;
        }
    }
}
