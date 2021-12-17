using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : ItemController
{
    [SerializeField] bool _onMove;
    [SerializeField] float _moveSpeed = 2;
    [SerializeField] int _mushroomScore = 1;

    float _angleValue;

    void Awake()
    {
        _myType = ItemType.Mushroom;

        _angleValue = transform.position.y;
    }

    void Update()
    {
        if (_onMove)
        {
            Move();
        }
    }

    void Move()
    {
        _angleValue += Time.deltaTime * _moveSpeed;
        transform.position = new Vector2(transform.position.x, Mathf.Sin(_angleValue));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController pc = collision.GetComponent<PlayerController>();

            GameManager.instance.UIManagerInstance.ScoreInstance.CheckScore(_mushroomScore);
            pc.TakeItem();

            Destroy(gameObject);
        }
    }
}
