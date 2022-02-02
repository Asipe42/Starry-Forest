using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [Header("Floor")]
    public List<GameObject> _floorPrefabs;
    public GameObject[] _nowWaitingFloors;
    [SerializeField] List<GameObject> _levelOneFloors;
    [SerializeField] List<GameObject> _levelTwoFloors;
    [SerializeField] List<GameObject> _levelThreeFloors;
    [SerializeField] List<GameObject> _preFloor;
    [SerializeField] GameObject _lastFloor;
    [SerializeField] float _moveSpeed;
    [SerializeField] float[] _accelerationValue;
    Vector3 _reposVec = new Vector3(106.5f, 0f, 0f);
    float _acceleration = 1f;
    bool _onLast;

    static bool _onStop;

    RunningBar runningBar;

    private void Awake()
    {
        _floorPrefabs = new List<GameObject>();
    }

    private void Start()
    {
        runningBar = GameManager.instance.UIManagerInstance.runningBarInstance;
    }

    void Update()
    {
        if (_onStop)
            return;

        Move();

        if (runningBar.GetBarPreValue() >= runningBar.GetBarMaxValue())
            Replace(true);
        else
            Replace(false);
    }

    public void ChangeFloors(Level level)
    {
        switch (level)
        {
            case Level.One:
                _floorPrefabs?.Clear();
                foreach (var floor in _levelOneFloors)
                    _floorPrefabs.Add(floor);
                break;
            case Level.Two:
                _floorPrefabs?.Clear();
                foreach (var floor in _levelTwoFloors)
                    _floorPrefabs.Add(floor);
                break;
            case Level.Three:
                Debug.Log("?");
                _floorPrefabs.Clear();
                foreach (var floor in _levelThreeFloors)
                    _floorPrefabs.Add(floor);
                break;
        }

        _nowWaitingFloors = _floorPrefabs.ToArray();
    }

    public static void StopFloorScrolling()
    {
        _onStop = true;
    }

    public static void ContinueFloorScrolling()
    {
        _onStop = false;
    }

    void Move()
    {
        if (GameManager.instance.StageManagerInstance.end)
            return;

        if (_onLast && _preFloor[_preFloor.Count - 1].gameObject.transform.position.x <= 0)
        {
            GameManager.instance.StageManagerInstance.EndGame();
            return;
        }

        Vector2 moveVec = Vector2.left;

        for (int i = 0; i < _preFloor.Count; i++)
            if (_preFloor[i] != null)
                _preFloor[i].transform.Translate(moveVec * _moveSpeed * _acceleration * Time.deltaTime);
    }

    void Replace(bool onLast)
    {
        if (_onLast)
            return;

        const float LIMIT_VALUE = -71.0f;

        for (int i = 0; i < _preFloor.Count; i++)
            if (_preFloor[i] != null)
                if (_preFloor[i].transform.position.x <= LIMIT_VALUE)
                { 
                    if (onLast)
                    {
                        _onLast = true;
                        CreateFloor(_preFloor[i].transform.position += _reposVec, true);
                        Destroy(_preFloor[i].gameObject);
                        _preFloor.RemoveAt(i);
                    }
                    else
                    {
                        CreateFloor(_preFloor[i].transform.position += _reposVec, false);
                        Destroy(_preFloor[i].gameObject);
                        _preFloor.RemoveAt(i);
                    }
                }
    }

    void CreateFloor(Vector2 _createPos, bool onLast)
    {
        if (onLast)
        {
            GameObject lastFloor = Instantiate(_lastFloor, transform);
            _preFloor.Add(lastFloor);
            lastFloor.transform.position = _createPos;
        }
        else
        {
            int floorIndex = UnityEngine.Random.Range(0, _nowWaitingFloors.Length);
            GameObject newFloor = Instantiate(_nowWaitingFloors[floorIndex], transform);
            _preFloor.Add(newFloor);
            newFloor.transform.position = _createPos;
        }
    }

    void CallLastFloor()
    {
        RunningBar runningBarLogic = GameManager.instance.UIManagerInstance.runningBarInstance;

        if (runningBarLogic.GetBarPreValue() >= runningBarLogic.GetBarMaxValue())
            Replace(true);
    }

    public void SetMoveValue(float value)
    {
        _moveSpeed = value;
    }

    public void OnAcceleration(DashSpace.DashLevel level)
    {
        switch (level)
        {
            case DashSpace.DashLevel.None:
                SetDefaultAcceleration();
                break;
            case DashSpace.DashLevel.One:
                _acceleration = _accelerationValue[0];
                break;
            case DashSpace.DashLevel.Two:
                _acceleration = _accelerationValue[1];
                break;
            case DashSpace.DashLevel.Three:
                _acceleration = _accelerationValue[2];
                break;
            case DashSpace.DashLevel.Max:
                _acceleration = _accelerationValue[3];
                break;
        }
    }

    public void SetDefaultAcceleration()
    {
        _acceleration = 1f;
    }
}
