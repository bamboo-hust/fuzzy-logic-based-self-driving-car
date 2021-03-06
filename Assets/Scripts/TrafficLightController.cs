﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    private const float RED_TIME = 5;
    private const float YELLO_TIME = 2;
    private const float GREEN_TIME = 5;
    private const float TOTAL_TIME = RED_TIME + YELLO_TIME + GREEN_TIME;

    private GameObject[] lights;
    private GameObject[] openLights;
    private float[] timestamps;
    private bool trafficLightParity;

    // Start is called before the first frame update
    void Start()
    {
        trafficLightParity = false;
        lights = Helper.GetTrafficLights();
        timestamps = new float[lights.Length];
        for (int i = 0; i < timestamps.Length; i++)
        {
            timestamps[i] = Random.Range(0.0f, TOTAL_TIME);
        }
    }

    public void FlipTrafficLightParity() {
        trafficLightParity = !trafficLightParity;
    }

    public void SetOpenLights(List<GameObject> openLights)
    {
        this.openLights = new GameObject[openLights.Count];
        for (int i = 0; i < openLights.Count; i++)
        {
            this.openLights[i] = openLights[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.IsPlaying()) return;

        UpdateLightStatus();
        UpdateOpenLightColliders();
    }

    void UpdateOpenLightColliders() {
        foreach (GameObject light in openLights) {
            if (!isGreen(light) && !trafficLightParity) {
                light.GetComponent<EdgeCollider2D>().isTrigger = false;
            } else {
                light.GetComponent<EdgeCollider2D>().isTrigger = true;
            }
        }
    }

    void UpdateLightStatus()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            timestamps[i] += Time.deltaTime;
            if (timestamps[i] > TOTAL_TIME) timestamps[i] -= TOTAL_TIME;
            if (timestamps[i] < RED_TIME)
            {
                SetTrafficLight(lights[i], "red");
            }
            else if (timestamps[i] < RED_TIME + GREEN_TIME)
            {
                SetTrafficLight(lights[i], "green");
            }
            else
            {
                SetTrafficLight(lights[i], "yellow");
            }
        }
    }

    void SetTrafficLight(GameObject light, string color)
    {
        for (int i = 0; i < light.transform.childCount; i++)
        {
            GameObject child = light.transform.GetChild(i).gameObject;
            if (child.name == "traffic-" + color) child.SetActive(true);
            else child.SetActive(false);
        }
    }

    bool isRed(GameObject light)
    {
        for (int i = 0; i < light.transform.childCount; i++)
        {
            GameObject child = light.transform.GetChild(i).gameObject;
            if (child.name == "traffic-red")
            {
                return child.activeSelf;
            }
        }
        return false;
    }

    bool isGreen(GameObject light) {
        for (int i = 0; i < light.transform.childCount; i++)
        {
            GameObject child = light.transform.GetChild(i).gameObject;
            if (child.name == "traffic-green")
            {
                return child.activeSelf;
            }
        }
        return false;
    }
}
