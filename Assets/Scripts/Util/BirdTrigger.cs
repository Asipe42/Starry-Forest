using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdTrigger : MonoBehaviour
{
    [SerializeField] Bird theBird;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        theBird._onDown = true;
        audioSource.Play();
    }
}
