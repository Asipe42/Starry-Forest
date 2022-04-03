using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vine vine = transform.parent.GetComponent<Vine>();

            vine.Appear(); vine.onAppear = true;
        }
    }
}
