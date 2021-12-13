using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    [SerializeField] private objectType _type;
    [SerializeField, Range(0, 1)] private float _blinkLevel;
    [SerializeField] private float _blinkSpeed;

    Image _myImage;

    SpriteRenderer spriteRenderer;

    enum objectType { None, Image, Sprite };
    float _blinkValue;
    bool _onAdd;

    Color _targetColor;

    private void Start()
    {
        switch (_type)
        {
            case objectType.Image:
                _myImage = GetComponent<Image>();
                break;
            case objectType.Sprite:
                spriteRenderer = GetComponent<SpriteRenderer>();
                break;
            default:
                Debug.Log("Not allocate object type");
                break;
        }
    }

    private void Update()
    {
        PlayBlink();
        AdjustValue();
    }

    private void PlayBlink()
    {
        switch (_type)
        {
            case objectType.Image:
                _targetColor = _myImage.color;
                _targetColor.a = 1 - _blinkValue;
                _myImage.color = _targetColor;
                break;
            case objectType.Sprite:
                _targetColor = spriteRenderer.color;
                _targetColor.a = 1 - _blinkValue;
                spriteRenderer.color = _targetColor;
                break;
            default:
                break;
        }

        if (_blinkValue > _blinkLevel)
        {
            _onAdd = false;
        }
    }

    private void AdjustValue()
    {
        if (_onAdd)
        {
            _blinkValue += Time.deltaTime * _blinkSpeed;
        }
        else
        {
            if (_blinkValue < 0)
            {
                _onAdd = true;
            }

            _blinkValue -= Time.deltaTime * _blinkSpeed;
        }
    }
}
