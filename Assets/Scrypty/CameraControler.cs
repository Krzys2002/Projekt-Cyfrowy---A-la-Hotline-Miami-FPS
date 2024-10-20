using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public Transform point;

    // Update is called once per frame
    void Update()
    {
        transform.position = point.position;
    }
}
