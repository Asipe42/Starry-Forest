using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image fillImage;
    [SerializeField, Range(1, 7)] int grade = 7;
    [SerializeField] int turningPoint;
    [SerializeField] float gauge;
    [SerializeField] float gaugeMax;
    [SerializeField] float[] fillSpeed;

    [Header("Color")]
    [SerializeField] Color[] colors;
    public bool[] changedColor;

    [Space]
    public static bool onLast;

    FloorManager floorManager;

    void Awake()
    {
        floorManager = GameObject.FindObjectOfType<FloorManager>().GetComponent<FloorManager>();

        slider.maxValue = gaugeMax;

        ChangeColor(colors[colors.Length - 1]);
    }

    void Update()
    {
        FillGague();
        CheckGauge();
    }

    void FillGague()
    {
        gauge += fillSpeed[(int)PlayerController.instance.dashLevel] * Time.deltaTime;
        slider.value = gauge;
    }

    void CheckGauge()
    {
        for (int i = 0; i < grade; i++)
        {
            if (changedColor[i])
                continue;

            if (slider.maxValue / (grade + 1) * (grade - i) <= slider.value)
            {
                if (i == turningPoint)
                    floorManager.LevelUp();

                changedColor[i] = true;
                ChangeColor(colors[i]);
            }
        }

        if (gauge >= slider.maxValue)
        {
            gauge = slider.maxValue;
            
            if (!onLast)
            {
                onLast = true;
            }
        }
    }

    void ChangeColor(Color nextColor)
    {
        fillImage.color = nextColor;
    }
}
