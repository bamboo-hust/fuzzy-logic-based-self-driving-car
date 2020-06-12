using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    private GameObject[] lights;
    private GameObject[] openLights;
    private float[] timestamps;
    private const float RED_TIME = 5;
    private const float YELLO_TIME = 2;
    private const float GREEN_TIME = 5;
    private const float TOTAL_TIME = RED_TIME + YELLO_TIME + GREEN_TIME;

    // Start is called before the first frame update
    void Start()
    {
        lights = Helper.GetTrafficLights();
        timestamps = new float[lights.Length];
        for (int i = 0; i < timestamps.Length; i++)
        {
            timestamps[i] = Random.Range(0.0f, TOTAL_TIME);
        }
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
            if (isRed(light)) {
                light.GetComponent<EdgeCollider2D>().enabled = true;
            } else {
                light.GetComponent<EdgeCollider2D>().enabled = false;
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
            else if (timestamps[i] < RED_TIME + YELLO_TIME)
            {
                SetTrafficLight(lights[i], "yellow");
            }
            else
            {
                SetTrafficLight(lights[i], "green");
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
}
