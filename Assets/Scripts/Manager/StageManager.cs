using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [SerializeField] int _index;
    public string _preSceneName;
    public string _nextSceneName;

    float _clearTime;

    bool _stop;
    bool _end;

    public float clearTime { get { return _clearTime; } }

    public bool end { get { return _end; } set { _end = value; } }
    public bool stop { get { return _stop; } set { _stop = value; } }

    void Update()
    {
        AddClearTime();
    }

    void AddClearTime()
    {
        if (!_end)
            _clearTime += Time.deltaTime;
    }

    void SaveValue(bool victory)
    {
        GameManager.instance.SaveTotalScore(GameManager.instance.UIManagerInstance.ScoreInstance.totalScore);
        GameManager.instance.SaveHp(GameManager.instance.PlayerControllerInstance._hp, GameManager.instance.PlayerControllerInstance._maxHp);

        if (victory)
            GameManager.instance.IncreaseSceneIndex(_index + 1);
        else
            GameManager.instance.IncreaseSceneIndex(_index);
    }

    /*void CheckDeathEvent()
    {
        if (GameManager.instance.GetDeathCount() == 1 && GameManager.instance.GetSceneIndex() == 0)
        {
            PlayDeathEvent(1);
        }
    }*/

    /*void PlayDeathEvent(int index)
    {
        end = true;
        GameManager.instance.UIManagerInstance.OnDialog();
        GameManager.instance.UIManagerInstance.DialogInstance.SetCondition(index);
    }*/

    public void GameOver()
    {
        //GameManager.instance.IncreaseDeathCount();

        SceneManager.LoadScene(_preSceneName);
    }

    public void Victory()
    {
        SaveValue(true);

        CallScene(_nextSceneName);
    }

    public void Retry()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(_preSceneName);
    }

    public void Title()
    {
        Time.timeScale = 1;
        CallScene("Title");
    }

    void CallScene(string sceneName)
    {
        LoadingSceneController.LoadScene(sceneName);
    }

    public void EndGame()
    {
        if (!_end)
        {
            _end = true;

            GameManager.instance.UIManagerInstance.SetResult(true);
        }
    }

    public void StartDialog()
    {
        GameManager.instance.UIManagerInstance.SetDialog(true);
        GameManager.instance.UIManagerInstance.DialogInstance.SetCondition(0);
    }

    public void StopGame()
    {
        _stop = true;
    }
}
