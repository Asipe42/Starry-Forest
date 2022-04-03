using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField] int score = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIManager.instance.theScore.CheckScore(score);

            collision.GetComponent<PlayerController>().TakeItem();

            Destroy(gameObject);
        }
    }
}
