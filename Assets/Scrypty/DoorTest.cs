using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTest : MonoBehaviour
{
    public float yScale = 1.0f;
    public float openTime = 1.0f;
    public Vector3 targetPosition;
    
    private void Start()
    {
        targetPosition = transform.position + new Vector3(0, yScale, 0);
    }

    public void OpenDoor()
    {
        StartCoroutine(SmoothMove(transform.position, targetPosition, openTime));
    }
    

    private IEnumerator SmoothMove(Vector3 start, Vector3 end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = end;
    }
}
