using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : ObstacleController
{
    [HideInInspector] public bool _onDown;
    [SerializeField] float _downSpeed = 10;
    [SerializeField] float _donwLimitPosition = -0.5f;

    private void Awake()
    {
        base.SetInfo();

        _myType = ObstacleType.Vine;
    }

    private void Update()
    {
        Down();
    }

    private void Down()
    {
        if (!_onDown)
            return;

        Vector2 downVec = new Vector2(transform.position.x, -0.4f);
        transform.position = Vector2.Lerp(transform.position, downVec, _downSpeed * Time.deltaTime);

        if (transform.position.y <= _donwLimitPosition)
            _onDown = false;
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
