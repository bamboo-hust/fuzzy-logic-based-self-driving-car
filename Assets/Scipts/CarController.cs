using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float maxRayCastDistance;

    private int trafficLightsParity;

    void Start()
    {
        trafficLightsParity = 0;
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.IsPlaying()) return;

        LayerMask wallMask = LayerMask.GetMask("Wall");
        LayerMask lightMask = LayerMask.GetMask("TrafficLight");
        List<LayerMask> masks = new List<LayerMask>();
        masks.Add(wallMask);
        masks.Add(lightMask);

        GameObject headSensor = transform.Find("Sensors/Front").gameObject;
        float distanceToWall = GetDistance(headSensor, masks);
        float translation = Mathf.Min(1.0f, distanceToWall * 10);
        translation *= speed;

        GameObject leftSensor = transform.Find("Sensors/Left").gameObject;
        GameObject rightSensor = transform.Find("Sensors/Right").gameObject;
        float distanceToLeftWall = GetDistance(leftSensor, masks);
        float distanceToRightWall = GetDistance(rightSensor, masks);

        if (distanceToWall < 0.01f || distanceToLeftWall < 0.01f || distanceToRightWall < 0.01f)
            translation = 0f;

        float rotation = distanceToLeftWall / (distanceToLeftWall + distanceToRightWall);
        rotation = rotation * 2.0f - 1.0f;
        rotation *= rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Rotate(0, 0, rotation);
        transform.position += transform.up * translation;
    }

    private float GetDistance(GameObject originPoint, List<LayerMask> masks)
    {
        float result = maxRayCastDistance;
        Vector2 hitPosition = Vector2.zero;
        foreach (LayerMask mask in masks)
        {
            RaycastHit2D hit = Physics2D.Raycast(originPoint.transform.position,
                originPoint.transform.up, maxRayCastDistance, mask);
            if (hit.collider == null) continue;
            else
            {
                float distance = Vector2.Distance(hit.point, originPoint.transform.position);
                if (distance < result)
                {
                    result = distance;
                    hitPosition = hit.point;
                }
            }
        }
        Debug.DrawLine((Vector2)originPoint.transform.position, (Vector2)hitPosition, Color.green);
        return result;
    }
}
