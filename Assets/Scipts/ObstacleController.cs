using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject obstaclePrefab;

    private GameObject obstacle;

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
            obstacle.transform.position = position;
            
            obstacle.SetActive(true);
        }
    }
}
