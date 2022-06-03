using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    // NOTICE: This is Legacy

    enum VisualType
    {
        Image,
        Sprite
    }

    [SerializeField] VisualType type;
    [SerializeField] float blinkSpeed = 0.4f;
    [SerializeField, Range(0, 1)] float limit = 0.5f;

    Image image;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        switch (type)
        {
            case VisualType.Image:
                image = GetComponent<Image>();
                break;
            case VisualType.Sprite:
                spriteRenderer = GetComponent<SpriteRenderer>();
                break;
        }
    }

    void Update()
    {
        switch (type)
        {
            case VisualType.Image:
                BlinkImage();
                break;
            case VisualType.Sprite:
                BlinkSprite();
                break;
        }
    }

    void BlinkImage()
    {
        Color _color;
        _color = image.color;

        if (_color.a > 1 || _color.a < limit)
            blinkSpeed = -blinkSpeed; // convert sign

        _color.a += blinkSpeed * Time.deltaTime;

        image.color = _color;
    }

    void BlinkSprite()
    {
        Color _color;
        _color = spriteRenderer.color;

        if (_color.a > 1 || _color.a < limit)
            blinkSpeed = -blinkSpeed; // convert sign

        _color.a += blinkSpeed * Time.deltaTime;

        spriteRenderer.color = _color;
    }
}
