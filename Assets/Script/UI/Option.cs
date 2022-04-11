using UnityEngine;

public class Option : MonoBehaviour
{
    [SerializeField] string currentSceneName;

    public void Restart()
    {
        Time.timeScale = 1f;
        Loading.LoadScene(currentSceneName);
    }

    public void OnSetting()
    {
        gameObject.SetActive(false);
        UIManager.instance.Activate_Popup_Children(Children_Popup.Setting);
    }

    public void GoTitle()
    {
        Time.timeScale = 1f;
        Loading.LoadScene("Title");
    }

    public void Cancle()
    {
        UIManager.instance.Activate_Popup_Children(Children_Popup.Option, false);
    }
}
