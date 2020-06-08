using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;

    void Start() {
        transform.position = target.position;
    }

    void FixedUpdate() {
        Vector3 targetCamPos = target.position;
        targetCamPos.z = -10;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
