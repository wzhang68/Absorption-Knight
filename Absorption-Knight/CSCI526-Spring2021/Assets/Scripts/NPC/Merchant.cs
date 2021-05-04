using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [SerializeField] private float Threshold;
    private GameObject Teodoro;
    void Start()
    {
        // Teodoro = GameObject.Find("Teodoro_ZLVer(Clone)");
        Teodoro = GameObject.Find("Teodoro_ZLVer");
    }
    private void Update()
    {
        if (Teodoro.transform.position.x > Threshold)
        {
            this.transform.localScale =new Vector3(1f, 1f, 1f);
        }
        else
        {
            this.transform.localScale =new Vector3(-1f, 1f, 1f);
        }
    }
}
