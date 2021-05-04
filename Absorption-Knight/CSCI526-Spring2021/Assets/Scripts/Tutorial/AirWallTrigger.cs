using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirWallTrigger : MonoBehaviour
{
    private GameObject wall;

    private void Start()
    {
        wall = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            wall.SetActive(false);
        }
    }
    
}
