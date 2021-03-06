﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkColliderHandler : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("TrafficLight")) return;
        GameManager.instance.GetComponent<TrafficLightController>().FlipTrafficLightParity();
    }
}
