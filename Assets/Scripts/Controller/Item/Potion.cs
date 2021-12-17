using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : ItemController
{
    [SerializeField] int startValue = 0;
    [SerializeField] int endValue = 3;
    [SerializeField] int _potionScore = 1;

    bool _onItem;

    void Awake()
    {
        _myType = ItemType.Potion;

        CreateRandomValue();
    }

    void Start()
    {
        CreatePotion();
    }

    void CreateRandomValue()
    {
        int ranValue = Random.Range(startValue, endValue);
        if (ranValue == 0)
            _onItem = true;
    }

    void CreatePotion()
    {
        gameObject.SetActive(false);

        if (_onItem)
            gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController pc = null;
        if (collision.gameObject.tag == "Player")
        {
            pc = collision.GetComponent<PlayerController>();

            pc.Recover(_potionScore);

            Destroy(gameObject);
        }
    }
}
