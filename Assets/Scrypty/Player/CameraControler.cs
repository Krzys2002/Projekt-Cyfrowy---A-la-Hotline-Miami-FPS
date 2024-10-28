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
        if(cameraPoint == null || mainCamera == null)
        {
            Debug.LogError("Camera or camera point not set.");
            return;
        }
        mainCamera.transform.position = cameraPoint.position;
    }
}
