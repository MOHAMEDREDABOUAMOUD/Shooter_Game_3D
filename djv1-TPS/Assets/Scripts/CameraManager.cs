using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCameraBase killCam;

    public void EnableKillCam()
    {
        killCam.Priority = 20;
    }
}
