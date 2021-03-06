﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Button startingButton;
    public Button endingButton;
    public Button goButton;
    public GameObject examplePoint;
    public GameObject carPrefab;
    public GameObject outsideWarning;
    public GameObject reachedDestinationText;
    public GameObject GoUI;

    private const float MIN_DISTANCE = 0.13f;

    private bool isPlaying = false;
    private bool reachedDestination = false;

    private Graph G;
    private bool isChooingStartingPoint;
    private bool isChooingEndingPoint;
    private GameObject startingPoint;
    private GameObject endingPoint;
    private Camera cam;
    private GameObject car;
    private Color startingColor = Color.red;
    private Color endingColor = Color.magenta;
    private GameObject secondPointInPath;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        cam = Camera.main;

        startingButton.GetComponent<Button>().onClick.AddListener(ClickStartingPoint);
        endingButton.GetComponent<Button>().onClick.AddListener(ClickEndingPoint);
        goButton.GetComponent<Button>().onClick.AddListener(ClickGo);
        car = Instantiate(carPrefab);
        car.SetActive(false);
        startingPoint = endingPoint = null;

        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(ChooingPoint());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());
    }

    private IEnumerator ChooingPoint()
    {
        SetDefaultCamera();
        while (startingPoint == null || endingPoint == null || !isPlaying)
        {
            if (Input.GetMouseButtonUp(0) && !isOverButtons())
            {
                if (isChooingStartingPoint)
                {
                    Vector2 clickedPosition = cam.ScreenToWorldPoint(Input.mousePosition);
                    if (!Helper.checkPointInRoad(clickedPosition))
                    {
                        outsideWarning.GetComponent<Animator>().SetTrigger("clickedOutside");
                    }
                    else SelectPoint(ref startingPoint, startingColor);
                }
                if (isChooingEndingPoint)
                {
                    Vector2 clickedPosition = cam.ScreenToWorldPoint(Input.mousePosition);
                    if (!Helper.checkPointInRoad(clickedPosition))
                    {
                        outsideWarning.GetComponent<Animator>().SetTrigger("clickedOutside");
                    }
                    else SelectPoint(ref endingPoint, endingColor);
                }
            }
            yield return null;
        }
    }

    private IEnumerator RoundPlaying()
    {
        SetupGraph();
        SetupTrafficLights();
        ConfigCamera();
        ConfigCar();
        DisablePoints();
        DeactiveButtons();
        while (Vector2.Distance(car.transform.position, endingPoint.transform.position) > MIN_DISTANCE)
        {
            yield return null;
        }
    }

    void DeactiveButtons() {
        startingButton.gameObject.SetActive(false);
        endingButton.gameObject.SetActive(false);
        goButton.gameObject.SetActive(false);
    }

    void SetupGraph()
    {
        GraphGenerator graphGenerator = new GraphGenerator(startingPoint, endingPoint);
        G = graphGenerator.Generate();
    }

    void SetupTrafficLights()
    {
        TurnAllTrafficLightCollidersOn();
        FindTrafficLightCollidersOnPath();
    }

    void FindTrafficLightCollidersOnPath()
    {
        List<GameObject> checkPoints = G.GetCheckPointsOnPath(startingPoint, endingPoint);

        secondPointInPath = checkPoints[1];

        List<GameObject> lights = G.GetOpenLights(checkPoints);

        GetComponent<TrafficLightController>().SetOpenLights(lights);
    }

    void TurnAllTrafficLightCollidersOn()
    {
        GameObject[] lights = Helper.GetTrafficLights();
        foreach (GameObject light in lights)
        {
            light.GetComponent<EdgeCollider2D>().enabled = true;
        }
    }

    void ConfigCar()
    {
        car.SetActive(true);
        car.transform.position = startingPoint.transform.position;

        Vector2 relativePos = secondPointInPath.transform.position - startingPoint.transform.position;
        relativePos.Normalize();
        float rot_z = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        car.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    void ConfigCamera()
    {
        cam.GetComponent<CameraController>().targerOrthographicSize = 1.0f;
        cam.GetComponent<CameraController>().SetFollowCamera(car);
    }

    void DisablePoints()
    {
        startingPoint.SetActive(false);
        endingPoint.transform.localScale /= 3.0f;
    }

    private IEnumerator RoundEnding()
    {
        isPlaying = false;
        reachedDestination = true;
        reachedDestinationText.GetComponent<Animator>().SetTrigger("reached");
        while (true) {
            if (Input.GetMouseButtonUp(0))
            {
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }
            yield return null;
        }
    }

    private void ClickStartingPoint()
    {
        isChooingStartingPoint = true;
        isChooingEndingPoint = false;
    }

    private void ClickEndingPoint()
    {
        isChooingStartingPoint = false;
        isChooingEndingPoint = true;
    }

    private void ClickGo()
    {
        if (isPlaying || reachedDestination) return;
        if (startingPoint == null || endingPoint == null)
        {
            GoUI.GetComponent<Animator>().SetTrigger("clickedOutside");
            return;
        }
        isPlaying = true;
    }

    private void SelectPoint(ref GameObject point, Color color)
    {
        if (point == null)
        {
            point = Instantiate(examplePoint);
            point.SetActive(true);
            GameObject child = point.transform.GetChild(0).gameObject;
            child.GetComponent<SpriteRenderer>().color = color;
        }
        Vector2 mousePosition = Input.mousePosition;
        Vector3 position = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0.0f));
        position.z = 0;
        point.transform.position = position;
    }

    public bool isOverButtons()
    {
        if (startingButton.GetComponent<UIElement>().mouseOver) return true;
        if (endingButton.GetComponent<UIElement>().mouseOver) return true;
        if (goButton.GetComponent<UIElement>().mouseOver) return true;
        return false;
    }

    private void SetDefaultCamera()
    {
        cam.transform.position = new Vector3(-1.68f, 0, -10);
        cam.orthographicSize = 4.9f;
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    public Vector3 GetCarPosition()
    {
        return car.transform.position;
    }
}
