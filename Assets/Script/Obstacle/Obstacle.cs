using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] AudioClip hitClip;

    public AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        hitClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Hit_01");
    }

    [SerializeField] int damage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().OnDamaged(damage);
            audioSource.PlayOneShot(hitClip);
        }
    }
}
