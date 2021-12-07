using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [Header("Floor")]
    [SerializeField] GameObject[] _floorPrefabs;
    [SerializeField] List<GameObject> _preFloor;
    [SerializeField] GameObject _lastFloor;
    [SerializeField] float _moveSpeed;
    [SerializeField] float[] _accelerationValue;
    Vector3 _reposVec = new Vector3(106.5f, 0f, 0f);
    float _acceleration = 1f;
    bool _onLast;

    RunningBar runningBar;

    private void Start()
    {
        runningBar = GameManager.instance.UIManagerInstance.runningBarInstance;
    }

    void Update()
    {
        Move();

        if (runningBar.GetBarPreValue() >= runningBar.GetBarMaxValue())
            Replace(true);
        else
            Replace(false);
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
        //const float REPOSITION_VALUE = 35.5f;
        //Vector2 createPoint = new Vector2(REPOSITION_VALUE, transform.position.y);

        if (onLast)
        {
            GameObject lastFloor = Instantiate(_lastFloor, transform);
            _preFloor.Add(lastFloor);
            lastFloor.transform.position = _createPos;
        }
        else
        {
            int floorIndex = Random.Range(0, _floorPrefabs.Length);
            GameObject newFloor = Instantiate(_floorPrefabs[floorIndex], transform);
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

    public void OnAcceleration(AccelerationSpace.AccelerationLevel level)
    {
        switch (level)
        {
            case AccelerationSpace.AccelerationLevel.None:
                SetDefaultAcceleration();
                break;
            case AccelerationSpace.AccelerationLevel.One:
                _acceleration = _accelerationValue[0];
                break;
            case AccelerationSpace.AccelerationLevel.Two:
                _acceleration = _accelerationValue[1];
                break;
            case AccelerationSpace.AccelerationLevel.Three:
                _acceleration = _accelerationValue[2];
                break;
            case AccelerationSpace.AccelerationLevel.Max:
                _acceleration = _accelerationValue[3];
                break;
        }
    }

    public void SetDefaultAcceleration()
    {
        _acceleration = 1f;
    }
}
