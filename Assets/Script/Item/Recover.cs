using UnityEngine;

public class Recover : MonoBehaviour
{
    [SerializeField] int recoverValue = 1;

    void Start()
    {
        CaculateRandomValue();
    }

    void CaculateRandomValue()
    {
        int randomValue = Random.Range(0, 10);

        if (randomValue == 0)
            Create(true); 
        else
            Create(false);
    }

    void Create(bool state)
    {
        gameObject.SetActive(state);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();

            playerController.Recover(recoverValue);

            Destroy(gameObject);
        }
    }
}
