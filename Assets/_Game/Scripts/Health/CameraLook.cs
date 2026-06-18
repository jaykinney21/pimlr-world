using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{

    public Transform mainCamera;

    void Start()
    {

        mainCamera = GameExecutionManager.Instance.cameraObj.transform;
    }

    void LateUpdate()
    {
        if (mainCamera != null)
            transform.LookAt(transform.position + mainCamera.forward);
    }

}
