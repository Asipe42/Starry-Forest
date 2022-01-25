using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : ObstacleController
{
    private void Awake()
    {
        base.SetInfo();

        _myType = ObstacleType.Stone;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();

            pc.Damaged(_info._damage, Definition.THORN_CLIP);

            GameManager.instance.UIManagerInstance.heartInstance.CheckHeart();
        }
    }
}
