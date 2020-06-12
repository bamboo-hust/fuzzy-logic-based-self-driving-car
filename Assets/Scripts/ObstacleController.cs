using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject obstaclePrefab;

    private GameObject obstacle;
    private const float MIN_DISTANCE = 0.5f;

    void Start()
    {
        obstacle = Instantiate(obstaclePrefab);
        obstacle.SetActive(false);
    }

    void Update()
    {
        if (!GameManager.instance.IsPlaying()) return;
        if (!Input.GetMouseButtonUp(0) || GameManager.instance.isOverButtons()) return;
        if (obstacle.activeSelf) obstacle.SetActive(false);
        else
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0.0f;
            
            if (Vector3.Distance(GameManager.instance.GetCarPosition(), position) < MIN_DISTANCE) {
                return;
            }        

            obstacle.transform.position = position;
            
            obstacle.SetActive(true);
        }
    }
}
