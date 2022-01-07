using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowCamController : MonoBehaviour
{
    [SerializeField] Transform[] _followTargetIndex;

    CinemachineVirtualCamera myCam;

    void Awake()
    {
        myCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void CancleFollowTarget()
    {
        myCam.Follow = null;
    }

    public void SetFollowTarget(int index)
    {
        myCam.Follow = _followTargetIndex[index];
    }
}
