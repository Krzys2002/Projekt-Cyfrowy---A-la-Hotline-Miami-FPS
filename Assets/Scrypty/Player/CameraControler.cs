using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControler))]
public class CameraControler : MonoBehaviour
{
    // Main camera is the camera that should be moved
    private Camera mainCamera;
    
    // Camera point is the point where the camera should be placed
    public Transform cameraPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get main camera
        mainCamera = GetComponent<PlayerControler>().cameraOrientation.gameObject.GetComponent<Camera>();
    }

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
