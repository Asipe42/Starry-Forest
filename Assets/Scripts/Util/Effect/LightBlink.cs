using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightBlink : MonoBehaviour
{
    Light2D light;

    [Header("Blink")]
    [SerializeField, Range(0, 1)] float _blinkSpeed;
    [SerializeField] float _intensityMaxValue;
    [SerializeField] float _intensityMinValue;
    [SerializeField] float _preIntensityValue;
    bool _onAdd = true;

    void Awake()
    {
        light = GetComponent<Light2D>();
    }

    void Start()
    {
        light.intensity = _preIntensityValue;
    }

    void Update()
    {
        PlayBlink();
        AdjustValue();
    }

    void PlayBlink()
    {
        light.intensity = _preIntensityValue;
    }

    void AdjustValue()
    {
        if (_onAdd)
        {
            _preIntensityValue += Time.deltaTime * _blinkSpeed;
            
            if (_preIntensityValue > _intensityMaxValue)
            {
                _onAdd = false;
            }
        }
        else
        {
            if (_preIntensityValue < _intensityMinValue)
            {
                _onAdd = true;
            }

            _preIntensityValue -= Time.deltaTime * _blinkSpeed;
        }
    }
}
