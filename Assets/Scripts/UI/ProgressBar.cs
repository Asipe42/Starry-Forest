using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image fillImage;
    [SerializeField] GameObject message;
    [SerializeField, Range(1, 7)] int grade = 7;
    [SerializeField] int turningPoint;
    [SerializeField] float gauge;
    [SerializeField] float gaugeMax;
    [SerializeField] float[] fillSpeed;

    [Header("Color")]
    [SerializeField] Color[] colors;
    public bool[] changedColor;
    public bool changedLevel;

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
        if (!PlayerController.instance.onTutorial)
        {
            if (slider.value == slider.maxValue)
                return;

            FillGague();
            CheckGauge();
        }
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
                {
                    if (!changedLevel)
                    {
                        changedLevel = true;
                        floorManager.LevelUp();
                    }
                }

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

                var sequence = DOTween.Sequence();

                sequence.Append(message.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce))
                        .AppendInterval(2f)
                        .Append(message.GetComponent<Image>().DOFade(0f, 1f))
                        .Append(message.GetComponentInChildren<Text>().DOFade(0f, 1f));       
            }
        }
    }

    void ChangeColor(Color nextColor)
    {
        fillImage.color = nextColor;
    }
}
