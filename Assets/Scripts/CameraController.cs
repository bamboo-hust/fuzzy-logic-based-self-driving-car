using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;

    public float targerOrthographicSize;

    void FixedUpdate()
    {
        if (!GameManager.instance.IsPlaying()) return;
        Vector3 targetCamPos = target.position;
        targetCamPos.z = -10;
        
        transform.position = Vector3.Lerp(transform.position, targetCamPos,
            smoothing * Time.deltaTime);

        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,
            targerOrthographicSize, smoothing * Time.deltaTime);
    }

    public void SetFollowCamera(GameObject car)
    {
        target = car.transform;
        transform.position = target.position;
    }
}
