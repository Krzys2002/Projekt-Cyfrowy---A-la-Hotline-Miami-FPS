using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public Camera mainCamera;
    public Transform cameraPoint;

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = cameraPoint.position;
    }
}
