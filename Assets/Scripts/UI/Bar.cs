using System.Collections;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] AudioClip clip;

    Vector3 destination;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator MoveBar(Vector3 destination)
    {
        this.destination = destination;
        audioSource.PlayOneShot(clip);

        while (transform.localPosition.y <= this.destination.y - 0.05 || transform.localPosition.y >= this.destination.y + 0.05)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, this.destination, Time.deltaTime * moveSpeed);

            yield return null;
        }

        Debug.Log("bar moving is done");
    }
}
