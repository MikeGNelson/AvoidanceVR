using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCam;
    public Transform targetA;
    public Transform targetB;

    public void SwitchToB()
    {
        virtualCam.LookAt = targetB;
    }

    void Start()
    {
        virtualCam.LookAt = targetA;
    }
}

