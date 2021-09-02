using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    [SerializeField] int _score;
    [SerializeField] bool _onMove;
    [SerializeField] float _moveSpeed;
    float _angleValue;

    private void Awake()
    {
        _angleValue = transform.position.y;
    }

    private void Update()
    {
        if (_onMove)
        {
            _angleValue += Time.deltaTime * _moveSpeed;
            _angleValue = Mathf.Clamp(_angleValue, -180, 180);
            Vector2 moveVec = new Vector2(transform.position.x, Mathf.Sin(_angleValue));
            transform.position = moveVec;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController pc = collision.GetComponent<PlayerController>();

            GameManager.instance.totalScore += _score;
            GameManager.instance.PlayScoreTextEffect();
            pc.TakeItem();
            Destroy(gameObject);
        }
    }
}
