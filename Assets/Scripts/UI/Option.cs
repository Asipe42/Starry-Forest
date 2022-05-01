using UnityEngine;

public class Option : MonoBehaviour
{
    [SerializeField] Setting setting;
    [SerializeField] string currentSceneName;

    public void Restart()
    {
        Time.timeScale = 1f;
        Loading.LoadScene(currentSceneName);
    }

    public void OnSetting()
    {
        gameObject.SetActive(false);
        UIManager.instance.onSetting = true;
        setting.gameObject.SetActive(true);
    }

    public void GoTitle()
    {
        Time.timeScale = 1f;
        Loading.LoadScene("Title");
    }

    public void Cancle()
    {
        UIManager.instance.ShowOption(false);
    }
}
