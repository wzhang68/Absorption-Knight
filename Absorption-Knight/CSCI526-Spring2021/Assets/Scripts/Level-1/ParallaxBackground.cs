using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Cinemachine;
using Vector3 = UnityEngine.Vector3;

public class ParallaxBackground : MonoBehaviour
{
    public Transform[] backgrounds;
    private float[] parrallaxScales;
    public float smoothing = 1f;

    private Camera myCamera;
    private Transform cam;
    private Vector3 previousCameraPos;


    private void Awake()
    {
        myCamera = FindObjectOfType<Camera>();
        cam = myCamera.transform;
    }


    // Start is called before the first frame update
    void Start()
    {
        previousCameraPos = cam.position;
        parrallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parrallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCameraPos.x - cam.position.x) * parrallaxScales[i];
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX,backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position =
                Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
            
        }

        previousCameraPos = cam.position;
        


    }


}
