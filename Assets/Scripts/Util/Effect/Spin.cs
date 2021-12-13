using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] float _spinSpeed;
    float _angleValue = 360f;

    void Update()
    {
        _angleValue -= Time.deltaTime;

        AdjustAngleValue();

        transform.rotation = Quaternion.Euler(0, 0, _angleValue * _spinSpeed);
    }

    void AdjustAngleValue()
    {
        if (_angleValue <= 0)
            _angleValue = 360f;
    }
}
