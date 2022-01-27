using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dandelion : ItemController
{

    void Awake()
    {
        _myType = ItemType.Dandelion;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMovement>()._onFly = true;

            GameManager.instance.AudioManagerInstance.PlaySFX(Definition.DANDELION_CLIP);

            Destroy(gameObject, 0.1f);
        }
    }
}
