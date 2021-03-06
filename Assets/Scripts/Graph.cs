﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph {
    public class Edge {
        public GameObject light;
        public int to;

        public Edge(int to, GameObject light) {
            this.to = to;
            this.light = light;
        }
    }
    public class Node {
        public GameObject point;
        public List<Edge> adj;
        public int id;

        public Node(int id, GameObject gameObject) {
            this.point = gameObject;
            this.id = id;
            adj = new List<Edge>();
        }
    }

    public List<Node> V;

    public Graph() {
        V = new List<Node>();
    }

    public void AddEdge(int from, int to, GameObject edge) {
        Debug.Log("addedge " + from + " " + to);
        V[from].adj.Add(new Edge(to, edge));
    }

    public void PrintGraph() {
        for (int i = 0; i < V.Count; ++i) {
            Debug.Log(V[i].point.name + ": ");
            for (int e = 0; e < V[i].adj.Count; ++e) {
                Debug.Log(V[V[i].adj[e].to].point.name + " | " + V[i].adj[e].light.name);
            }
        }
    }

    public List<int> FindPath(int from, int to) {
        Queue queue = new Queue();
        queue.Enqueue(from);
        int[] trace = new int[V.Count];
        for (int i = 0; i < V.Count; ++i) trace[i] = -1;
        trace[from] = from;
        while (queue.Count > 0) {
            int u = (int) queue.Dequeue();
            for (int i = 0; i < V[u].adj.Count; ++i) {
                int v = V[u].adj[i].to;
                Debug.Log("edge " + u + " " + v);
                if (trace[v] < 0) {
                    trace[v] = u;
                    queue.Enqueue(v);
                }
            }
        }
        for (int i = 0; i < V.Count; ++i) {
            Debug.Log("trace " + i + " " + trace[i]);
        }
        if (trace[to] == -1) return new List<int>();
        List<int> path = new List<int>();
        for (int u = to; u != from; u = trace[u]) {
            path.Add(u);
        }
        path.Add(from);
        path.Reverse();
        foreach (int u in path) {
            Debug.Log("path " + u);
        }
        return path;
    }

    public int GetId(GameObject point) {
        for (int i = 0; i < V.Count; ++i) {
            if (V[i].point == point) return i;
        }
        return -1;
    }

    public List<GameObject> GetCheckPointsOnPath(GameObject from, GameObject to) {
        int from_id = GetId(from);
        int to_id = GetId(to);
        List<GameObject> res = new List<GameObject>();
        if (from_id < 0 || to_id < 0) return res;
        List<int> path = FindPath(from_id, to_id);
        if (path.Count == 0) return res;
        for (int i = 0; i < path.Count; ++i) {
            res.Add(V[path[i]].point);
        }
        return res;
    }

    public List<GameObject> GetOpenLights(List<GameObject> checkPointsList) {
        List<GameObject> res = new List<GameObject>();
        for (int i = 1; i < checkPointsList.Count; ++i) {
            Vector2 from_point = checkPointsList[i - 1].transform.position;
            Vector2 to_point = checkPointsList[i].transform.position;
            List<GameObject> cur = Helper.ExtractTrafficLights(Helper.GetIntersections(from_point, to_point));
            foreach (GameObject o in cur) {
                res.Add(o);
            }
        }
        return res;
    }
}
