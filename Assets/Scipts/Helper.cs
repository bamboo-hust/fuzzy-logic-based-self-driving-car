using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper {

    public static RaycastHit2D[] GetIntersections(Vector2 A, Vector2 B) {
        Vector2 diff = B - A;
        RaycastHit2D[] hits = Physics2D.RaycastAll(A, diff, diff.magnitude);
        return hits;
    }
    
    public static bool HasRoad(RaycastHit2D[] hit2Ds) {
        GameObject roadCollider = GameObject.Find("RoadCollider");
        for (int i = 0; i < hit2Ds.Length; ++i) {
            if (hit2Ds[i].transform.gameObject == roadCollider) {
                return true;
            }
        }
        return false;
    }

    public static bool IntersectRoad(Vector2 A, Vector2 B) {
        return HasRoad(GetIntersections(A, B));
    }

    public static GameObject[] GetCheckPoints() {
        Transform checkPointsTransform = GameObject.Find("CheckPoints").transform;
        GameObject[] checkPoints = new GameObject[checkPointsTransform.childCount];
        for (int i = 0; i < checkPointsTransform.childCount; ++i) {
            Transform childTransform = checkPointsTransform.GetChild(i);
            checkPoints[i] = childTransform.gameObject;
        }
        return checkPoints;
    }

    public static GameObject[] GetTrafficLights() {
        Transform trafficLightsTransform = GameObject.Find("TrafficLight").transform;
        GameObject[] trafficLights = new GameObject[trafficLightsTransform.childCount];
        for (int i = 0; i < trafficLightsTransform.childCount; ++i) {
            Transform childTransform = trafficLightsTransform.GetChild(i);
            trafficLights[i] = childTransform.gameObject;
        }
        return trafficLights;
    }

    bool checkPointInRoad(Vector2 O) {
        GameObject[] checkPoints = GetCheckPoints();
        for (int i = 0; i < checkPoints.Length; ++i) {
            if (!IntersectRoad(O, checkPoints[i].transform.position)) {
                return true;
            }
        }
        return false;
    }
}
