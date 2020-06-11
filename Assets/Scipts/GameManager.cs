using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button startingButton;
    public Button endingButton;
    public Button goButton;
    public GraphGenerator graphGenerator;

    private bool isPlaying = false;
    private bool reachedDestination = false;

    private bool isChooingStartingPoint;
    private bool isChooingEndingPoint;
    private GameObject startingPoint;
    private GameObject endingPoint;


    private void Start()
    {
        graphGenerator = new GraphGenerator();
        Graph G = graphGenerator.Generate();
        G.PrintGraph();
        startingButton.GetComponent<Button>().onClick.AddListener(ClickStartingPoint);
        endingButton.GetComponent<Button>().onClick.AddListener(ClickEndingPoint);
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
        while (startingPoint == null || endingPoint == null)
        {

        }
        return null;
    }

    private IEnumerator RoundPlaying()
    {
        return null;
    }

    private IEnumerator RoundEnding()
    {
        return null;
    }

    private void ClickStartingPoint()
    {
        Debug.Log("start");
    }

    private void ClickEndingPoint()
    {
        Debug.Log("start");
    }
}
