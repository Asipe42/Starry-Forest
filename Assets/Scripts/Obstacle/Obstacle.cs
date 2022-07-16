using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] protected int damage;

    protected AudioClip hitClip;

    protected void CheckTrigger(out bool reachPlayer, Transform start, Transform end, LayerMask whatIsPlayer)
    {
        reachPlayer = Physics2D.Linecast(start.position, end.position, whatIsPlayer);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            playerController.OnDamaged(damage, hitClip);
        }
    }
}
