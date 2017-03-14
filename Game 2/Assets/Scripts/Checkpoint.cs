﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public LevelManager levelManager;


    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Checkpoint Activated");
            levelManager.currentCheckPoint = gameObject;
        }
    }
}
