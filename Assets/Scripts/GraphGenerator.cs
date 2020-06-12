using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGenerator
{
    private Transform transform;
    private GameObject[] checkPoints;
    private GameObject[] trafficLights;
    private GameObject roadCollider;

    public GraphGenerator() {
        checkPoints = Helper.GetCheckPoints();
        trafficLights = Helper.GetTrafficLights();
        roadCollider = GameObject.Find("RoadCollider");
    }

    public GraphGenerator(GameObject startingPoint, GameObject endingPoint) {
        GameObject[] listCheckPoints = Helper.GetCheckPoints();
        checkPoints = new GameObject[listCheckPoints.Length + 2];
        checkPoints[0] = startingPoint;
        for (int i = 0; i < listCheckPoints.Length; i++)
        {
            checkPoints[i + 1] = listCheckPoints[i];
        }
        checkPoints[checkPoints.Length - 1] = endingPoint;
        trafficLights = Helper.GetTrafficLights();
        roadCollider = GameObject.Find("RoadCollider");
    }

    public Graph Generate() {
        Graph G = new Graph();
        for (int i = 0; i < checkPoints.Length; ++i) {
            G.V.Add(new Graph.Node(i, checkPoints[i]));
        }
        // Debug.Log("graph size = " + G.V.Count);
        for (int i = 0; i < checkPoints.Length; ++i) {
            Dictionary<int, int> closest = new Dictionary<int, int>();
            for (int j = 0; j < checkPoints.Length; ++j) if (i != j) {
                RaycastHit2D[] hits = GetIntersections(checkPoints[i], checkPoints[j]);
                if (Helper.HasRoad(hits)) {
                    continue;
                }
                int nearestLight = -1;
                for (int k = 0; k < trafficLights.Length; ++k) {
                    if (HasTrafficLight(hits, trafficLights[k])) {
                        if (nearestLight == -1 || GetDist(checkPoints[i], trafficLights[nearestLight]) > GetDist(checkPoints[i], trafficLights[k])) {
                            nearestLight = k;
                        }
                    }
                }
                if (nearestLight != -1) {
                    if (closest.ContainsKey(nearestLight)) {
                        if (GetDist(checkPoints[i], checkPoints[j]) < GetDist(checkPoints[i], checkPoints[closest[nearestLight]])) {
                            closest[nearestLight] = j;
                        }
                    } else {
                        closest[nearestLight] = j;
                    }
                } else {
                    Debug.Log("DIRECT EDGE " + i + " " + j);
                    G.AddEdge(i, j, new GameObject("dummy"));
                }
            }
            foreach (int k in closest.Keys) {
                G.AddEdge(i, closest[k], trafficLights[k]);
            }
        }
        return G;
    }

    RaycastHit2D[] GetIntersections(GameObject A, GameObject B) {
        return Helper.GetIntersections(A.transform.position, B.transform.position);
    }

    bool HasTrafficLight(RaycastHit2D[] hit2Ds, GameObject light) {
        for (int i = 0; i < hit2Ds.Length; ++i) {
            if (hit2Ds[i].transform.gameObject == light) {
                return true;
            }
        }
        return false;
    }
    
    float GetDist(GameObject A, GameObject B) {
        return (A.transform.position - B.transform.position).magnitude;
    }
}
