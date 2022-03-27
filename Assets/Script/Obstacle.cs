using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] AudioClip hitClip;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = hitClip;
    }

    [SerializeField] int damage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().OnDamaged(damage);
            audioSource.Play();
        }
    }
}
