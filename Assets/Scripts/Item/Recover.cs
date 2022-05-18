using UnityEngine;

public class Recover : MonoBehaviour
{
    [SerializeField] int recoverValue = 1;
    [SerializeField] bool onRandom = true;

    #region Set Random Activation
    void Start()
    {
        if (onRandom)
        {
            SetRandomActivation();
        }
    }

    void SetRandomActivation()
    {
        bool state = CalculateRandomValue();

        SetActivation(state);
    }

    bool CalculateRandomValue()
    {
        int randomValue = Random.Range(0, 10);

        if (randomValue == 0)
            return true;
        else
            return false;
    }

    void SetActivation(bool state)
    {
        gameObject.SetActive(state);
    }
    #endregion

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
