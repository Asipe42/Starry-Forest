using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    None,
    Thron,
    Vine,
    Stone,
    FloatingWood
}

public class ObstacleInfo
{
    public int _damage;
}

public class ObstacleController : MonoBehaviour
{
    [SerializeField] protected ObstacleType _myType = ObstacleType.None;
    [SerializeField] int _thronDamage = 1;
    [SerializeField] int _vineDamage = 1;
    [SerializeField] int _stoneDamage = 1;
    [SerializeField] int _floatingWoodDamage = 1;

    protected ObstacleInfo _info = new ObstacleInfo();

    protected void SetInfo()
    {
        switch (_myType)
        {
            case ObstacleType.Thron:
                _info._damage = _thronDamage;
                break;
            case ObstacleType.Vine:
                _info._damage = _vineDamage;
                break;
            case ObstacleType.Stone:
                _info._damage = _stoneDamage;
                break;
            case ObstacleType.FloatingWood:
                _info._damage = _floatingWoodDamage;
                break;
        }
    }
}
