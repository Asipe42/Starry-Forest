using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] Image _fadePanel;
    [SerializeField, Range(0, 1)] float _fadeGaugeValue;
    float _alphaValue;

    void Update()
    {
        _alphaValue += Time.deltaTime;

        _fadePanel.color = new Color(0, 0, 0, 1 - _alphaValue * _fadeGaugeValue);

        if (_alphaValue * _fadeGaugeValue > 1)
            Destroy(gameObject);
    }
}
