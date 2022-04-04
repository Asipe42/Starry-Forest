using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = "Dash Level: " + PlayerController.instance.dashLevel;
    }
}
