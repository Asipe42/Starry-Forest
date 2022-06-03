using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] int damage;

    protected AudioClip hitClip;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            playerController.OnDamaged(damage, hitClip);
        }
    }
}
