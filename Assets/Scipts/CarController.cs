using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float maxRayCastDistance;

    void Start()
    {

    }

    void Update()
    {
        LayerMask mask = LayerMask.GetMask("Wall");
        GameObject headSensor = transform.Find("Sensors/Front").gameObject;
        float distanceToWall = GetDistance(headSensor, mask);
        float translation = Mathf.Min(1.0f, distanceToWall * 10);
        translation *= speed;

        GameObject leftSensor = transform.Find("Sensors/Left").gameObject;
        GameObject rightSensor = transform.Find("Sensors/Right").gameObject;
        float distanceToLeftWall = GetDistance(leftSensor, mask);
        float distanceToRightWall = GetDistance(rightSensor, mask);
        
        float rotation = distanceToLeftWall / (distanceToLeftWall + distanceToRightWall);
        rotation = rotation * 2.0f - 1.0f;
        rotation *= rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Rotate(0, 0, rotation);
        transform.position += transform.up * translation;
    }

    private float GetDistance(GameObject originPoint, LayerMask mask)
    {
        RaycastHit2D hit = Physics2D.Raycast(originPoint.transform.position,
            originPoint.transform.up, maxRayCastDistance, mask);
        if (hit.collider == null) return maxRayCastDistance;
        else 
        {
            Debug.DrawLine(originPoint.transform.position, hit.point,
                Color.green);
            return Vector2.Distance(hit.point, originPoint.transform.position);
        }
    }
}
