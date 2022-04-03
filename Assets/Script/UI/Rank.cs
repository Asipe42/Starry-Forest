using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Rank : MonoBehaviour
{
    [SerializeField] Image rankEmpty;
    [SerializeField] Image rankFull;
    [SerializeField] TextMeshProUGUI timeText;

    [SerializeField] Sprite[] rankSpriteFull;
    [SerializeField] Sprite[] rankSpriteEmtpy;

    float time = 0f;

    void Update()
    {
        DisplayTime();
    }

    void DisplayTime()
    {
        time += Time.deltaTime;
        timeText.text = string.Format("{0:0.0}", time);
    }
}
