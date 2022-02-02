using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Level
{
    None,
    One,
    Two,
    Three,
}

public class RunningBar : MonoBehaviour
{
    [Header("Running Bar")]
    [SerializeField] Slider _runningBar;
    [SerializeField] Image _fillImage;
    [SerializeField, Range(1, 7)] int _grade = 7;
    [SerializeField] float[] _speedValues = new float[7];
    [SerializeField] float _runningGague;
    [SerializeField] float _runningGagueMaxValue;
    [SerializeField] float[] _fillSpeedValue;
    float _fillSpeed = 1f;
    bool[] _changedColor = new bool[7];
    bool _runinng;
    Level _myLevel = Level.None;

    public float progressPercentage
    {
        get
        {
            return (int)((_runningGague / _runningGagueMaxValue) * 100);
        }
    }

    [Header("Bar Colors")]
    [SerializeField] Color32[] _nextColors = new Color32[7];

    void Awake()
    {
        _runningBar.maxValue = _runningGagueMaxValue;
    }

    void Update()
    {
        IncreaseGaugeValue();
        CheckRunningBar();
        CheckLevel();
    }

    void IncreaseGaugeValue()
    {
        _runningGague += Time.deltaTime * _fillSpeed;
    }

    void CheckRunningBar()
    {
        _runningBar.value = _runningGague;

        for (int i = 0; i < _grade; i++)
        {
            if (_changedColor[i])
                continue;

            if (_runningBar.maxValue / (_grade + 1) * (_grade - i) <= _runningBar.value)
            {
                _changedColor[i] = true;
                ChangeBarColor(_nextColors[i]);
                SpeedUpFloor(_speedValues[_speedValues.Length - (i + 1)]);
            }
        }

        if (_runningGague >= _runningBar.maxValue)
        {
            _runningGague = _runningBar.maxValue;

             if (!GameManager.instance.StageManagerInstance.stop)
                GameManager.instance.StageManagerInstance.StopGame();
        } 
    }

    void ChangeBarColor(Color32 nextColor)
    {
        _fillImage.color = nextColor;
    }

    void SpeedUpFloor(float value)
    {
        GameManager.instance.FloorManagerInstance.SetMoveValue(value);
    }

    void CheckLevel()
    {
        if (_runningGague > _runningGagueMaxValue * 3f / 4f)
        {
            if (_myLevel != Level.Three)
            {
                _myLevel = Level.Three;
                GameManager.instance.FloorManagerInstance.ChangeFloors(Level.Three);
            }
        }
        else if (_runningGague > _runningGagueMaxValue * 2f / 4f)
        {
            if (_myLevel != Level.Two)
            {
                _myLevel = Level.Two;
                GameManager.instance.FloorManagerInstance.ChangeFloors(Level.Two);
            }
        }
        else
        {
            if (_myLevel != Level.One)
            {
                _myLevel = Level.One;
                GameManager.instance.FloorManagerInstance.ChangeFloors(Level.One);
            }
        }
    }

    public float GetBarPreValue()
    {
        return _runningBar.value;
    }

    public float GetBarMaxValue()
    {
        return _runningBar.maxValue;
    }

    public void IncreaseFillSpeed(DashSpace.DashLevel level)
    {
        switch (level)
        {
            case DashSpace.DashLevel.None:
                SetDefaultFillSpeed();
                break;
            case DashSpace.DashLevel.One:
                _fillSpeed = _fillSpeedValue[0];
                break;
            case DashSpace.DashLevel.Two:
                _fillSpeed = _fillSpeedValue[1];
                break;
            case DashSpace.DashLevel.Three:
                _fillSpeed = _fillSpeedValue[2];
                break;
            case DashSpace.DashLevel.Max:
                _fillSpeed = _fillSpeedValue[3];
                break;
        }
    }

    public void SetDefaultFillSpeed()
    {
        _fillSpeed = 1f;
    }

    void UpLevel(Level level)
    {

    }
}
