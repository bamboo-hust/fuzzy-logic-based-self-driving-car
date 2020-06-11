using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph {
    public class Edge {
        public GameObject light;
        public Node to;

        public Edge(Node to, GameObject light) {
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
        V[from].adj.Add(new Edge(V[to], edge));
    }

    public void PrintGraph() {
        for (int i = 0; i < V.Count; ++i) {
            Debug.Log(V[i].point.name + ": ");
            for (int e = 0; e < V[i].adj.Count; ++e) {
                Debug.Log(V[i].adj[e].to.point.name + " | " + V[i].adj[e].light.name);
            }
        }
    }
}
