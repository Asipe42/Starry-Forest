using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject Dialog;
    [SerializeField] GameObject Result;
    [SerializeField] GameObject Guide;
    [SerializeField] GameObject Option;

    RunningBar _runningBar;
    Heart _heart;
    Score _score;
    Blood _blood;
    Dialog _dialog;
    Result _result;
    Option _option;

    public RunningBar runningBarInstance { get { return _runningBar; } }
    public Heart heartInstance { get { return _heart; } }
    public Score ScoreInstance { get { return _score; } }
    public Blood BloodInstance { get { return _blood; } }
    public Dialog DialogInstance { get { return _dialog; } }
    public Result ResultInstance { get { return _result; } }

    private void Awake()
    {
        _runningBar = GameObject.Find("RunningBox").GetComponent<RunningBar>();
        _heart = GameObject.Find("HeartBox").GetComponent<Heart>();
        _score = GameObject.Find("ScoreBox").GetComponent<Score>();
        _blood = GameObject.Find("BloodBox").GetComponent<Blood>();

        _dialog = Dialog.GetComponent<Dialog>();
        _result = Result.GetComponent<Result>();
        _option = Option.GetComponent<Option>();
    }

    public void SetDialog(bool state)
    {
        Result.SetActive(!state);

        Dialog.SetActive(state);
    }

    public void SetHUD(bool state)
    {
        Dialog.SetActive(!state);

        HUD.SetActive(state);
    }

    public void SetResult(bool state)
    {
        HUD.SetActive(!state);

        Result.SetActive(state);

        if (state)
            _result.SetResult();
    }

    public void SetOption(bool state)
    {
        Option.SetActive(state);
        
        if (state)
        {
            GameManager.instance.GameStop();
        }
        else
        {
            GameManager.instance.GameContinue();
        }
    }
}
