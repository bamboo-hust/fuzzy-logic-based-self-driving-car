using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;

    void FixedUpdate() {
        if (!GameManager.instance.getIsPlaying()) return;
        Vector3 targetCamPos = target.position;
        targetCamPos.z = -10;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }

    public void SetFollowCamera(GameObject car) {
        target = car.transform;
        transform.position = target.position;
    }
}
