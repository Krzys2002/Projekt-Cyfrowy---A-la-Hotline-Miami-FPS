using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    // Main camera is the camera that should be moved
    public Camera mainCamera;
    
    // Camera point is the point where the camera should be placed
    public Transform cameraPoint;

    // Update is called once per frame
    void Update()
    {
        // Check if camera point or main camera is not set
        if(cameraPoint == null || mainCamera == null)
        {
            // Log error
            Debug.LogError("Camera or camera point not set.");
            return;
        }
        // Set camera position to camera point position
        mainCamera.transform.position = cameraPoint.position;
    }
}
