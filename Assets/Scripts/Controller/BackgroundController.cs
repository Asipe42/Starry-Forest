using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] Transform[] _firstLayer;
    [SerializeField] Transform[] _secondLayer;
    [SerializeField] float[] _moveSpeed;

    const float LIMIT_VALUE = -71.0f;
    Vector3 _reposVec = new Vector3(106.5f, 0f, 0f);

    static bool _onStop;

    private void Update()
    {
        if (_onStop)
            return;

        MoveLayer();
        Reposition();
    }

    public static void StopBackgroundScrolling()
    {
        _onStop = true;
    }

    public static void ContinueBackgroundScrolling()
    {
        _onStop = false;
    }

    private void MoveLayer()
    {
        if (GameManager.instance.StageManagerInstance.end)
            return;

        Vector2 moveVec = Vector2.left;

        for (int i = 0; i < _firstLayer.Length; i++)
            _firstLayer[i].Translate(moveVec * _moveSpeed[0] * Time.deltaTime);

        for (int i = 0; i < _secondLayer.Length; i++)
            _secondLayer[i].Translate(moveVec * _moveSpeed[1] * Time.deltaTime);
    }

    private void Reposition()
    {
        if (GameManager.instance.StageManagerInstance.end)
            return;

        for (int i = 0; i < _firstLayer.Length; i++)
            if (_firstLayer[i].position.x <= LIMIT_VALUE)
                _firstLayer[i].position += _reposVec;

        for (int i = 0; i < _secondLayer.Length; i++)
            if (_secondLayer[i].position.x <= LIMIT_VALUE)
                _secondLayer[i].position += _reposVec;

        //_secondLayer[i].position = new Vector2(REPOSITION_VALUE, 0);
    }
}
