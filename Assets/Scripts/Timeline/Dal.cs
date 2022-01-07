using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dal : MonoBehaviour
{
    Animator anim;
    AudioSource audioSource;

    public enum MoveDirection
    {
        Stop,
        Left,
        Right
    }
    MoveDirection _myMoveDirection;
    [SerializeField] float _moveSpeed;
    bool _onMove;

    void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Move();
    }

    public void SelectDirection(int direction)
    {
        _myMoveDirection = (MoveDirection)direction;
    }

    void Move()
    {
        switch (_myMoveDirection)
        {
            case MoveDirection.Left:
            case MoveDirection.Right:
                MoveLogic(_myMoveDirection);
                break;

            case MoveDirection.Stop:
                if (_onMove)
                {
                    _onMove = false;
                    anim.SetBool("idle", true);
                    anim.SetBool("walk", false);
                    audioSource.Stop();
                }
                break;
        }
    }

    void MoveLogic(MoveDirection direction)
    {
        if (!_onMove)
        {
            _onMove = true;

            anim.SetBool("walk", true);
            audioSource.Play();

            if (direction == MoveDirection.Left)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
        }

        if (direction == MoveDirection.Left)
        {
            transform.Translate(Vector2.left * _moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * _moveSpeed * Time.deltaTime);
        }
    }
}
