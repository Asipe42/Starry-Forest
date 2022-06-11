using UnityEngine;

public class WoodTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Wood wood = transform.parent.GetComponent<Wood>();

            wood.Appear(); wood.onAppear = true;
        }
    }
}
