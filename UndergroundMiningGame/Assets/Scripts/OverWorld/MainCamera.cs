﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    GameObject player;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = new Vector3(0, 0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject transitions = GameObject.FindGameObjectWithTag("TransitionManager");
        transitions.GetComponent<Canvas>().worldCamera = Camera.main;
        transform.position = player.transform.position + offset;
    }
}
