using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFloating : MonoBehaviour
{
    private Vector3 localPos;
    public int maxSpeed;
    
    private void Awake()
    { 
        localPos = transform.position;

    }

    void Update()
    {
        moveVertical();
    }

    private void moveVertical()
    {
        transform.position = new Vector3(localPos.x, localPos.y + 0.1f * Mathf.Sin(Time.time * maxSpeed), 0);
    }
}
