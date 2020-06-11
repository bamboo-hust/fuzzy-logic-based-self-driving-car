using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Button startingButton;
    public Button endingButton;
    public Button goButton;
    public GraphGenerator graphGenerator;
    public GameObject examplePoint;
    public GameObject carPrefab;
    public GameObject outsideWarning;

    private bool isPlaying = false;
    private bool reachedDestination = false;

    private bool isChooingStartingPoint;
    private bool isChooingEndingPoint;
    private GameObject startingPoint;
    private GameObject endingPoint;
    private Camera cam;
    private GameObject car;
    private Color startingColor = Color.red;
    private Color endingColor = Color.magenta;

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
        graphGenerator = new GraphGenerator();
        Graph G = graphGenerator.Generate();

        cam = Camera.main;

        startingButton.GetComponent<Button>().onClick.AddListener(ClickStartingPoint);
        endingButton.GetComponent<Button>().onClick.AddListener(ClickEndingPoint);
        goButton.GetComponent<Button>().onClick.AddListener(ClickGo);
        car = Instantiate(carPrefab);
        car.SetActive(false);

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
        ConfigCar();
        ConfigCamera();
        DisablePoints();
        while (true) yield return null;
    }

    void ConfigCar()
    {
        car.SetActive(true);
        car.transform.position = startingPoint.transform.position;

        Vector2 relativePos = endingPoint.transform.position - startingPoint.transform.position;
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
        return null;
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
        if (startingPoint == null || endingPoint == null)
        {
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
        Debug.Log(point.transform.position);
    }

    bool isOverButtons()
    {
        if (startingButton.GetComponent<UIElement>().mouseOver) return true;
        if (endingButton.GetComponent<UIElement>().mouseOver) return true;
        if (goButton.GetComponent<UIElement>().mouseOver) return true;
        return false;
    }

    private void SetDefaultCamera()
    {
        cam.transform.position = new Vector3(0, 0, -10);
        cam.orthographicSize = 4.9f;
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }
}
