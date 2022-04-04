using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] AudioClip hitClip;

    void Awake()
    {
        hitClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Hit_01");
    }

    [SerializeField] int damage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            playerController.OnDamaged(damage, hitClip);
        }
    }
}
