using UnityEngine;

public class SnakeTrigger : MonoBehaviour
{
    [SerializeField] GameObject SnakeObject;
    [SerializeField] float[] snakeAppearProgressValueRatio;

    bool[] onApeear;

    ProgressBar progressBar;

    void Awake()
    {
        Initialize();
    }

    void Update()
    {
        MonitorProgressValue();
    }

    #region Initial Setting
    void Initialize()
    {
        progressBar = UIManager.instance.progressBar;

        onApeear = new bool[snakeAppearProgressValueRatio.Length];
    }
    #endregion

    void MonitorProgressValue()
    {
        for (int i = 0; i < snakeAppearProgressValueRatio.Length; i++)
        {
            if (onApeear[i])
                continue;

            if (progressBar.Gauge >= progressBar.GaugeMax / 100 * snakeAppearProgressValueRatio[i])
            {
                onApeear[i] = true;
                SnakeObject.SetActive(true);
            }
        }
    }
}
