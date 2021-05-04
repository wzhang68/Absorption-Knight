using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CaneraClamp_AllLevel : MonoBehaviour
{
    private Camera _camera;
    private Transform targetToFollow;
    [Header("Map Boundaries")]
    [SerializeField] private float leftLimit;
    [SerializeField] private float rightLimit;
    [SerializeField] private float topLimit;
    [SerializeField] private float bottomLimit;
    [SerializeField] private float smoothing;

    
    
    private void Start()
    {
        _camera = GetComponent<Camera>();
        targetToFollow = GameObject.Find("Teodoro_ZLVer").transform;
        // targetToFollow = GameObject.Find("Teodoro_ZLVer(Clone)").transform;
    }

    void LateUpdate()
    {
        if (_camera.transform.position != targetToFollow.transform.position)
        {
            Vector3 targetPos = targetToFollow.position;
            targetPos.x = Mathf.Clamp(targetPos.x, leftLimit, rightLimit);
            targetPos.y = Mathf.Clamp(targetPos.y, bottomLimit, topLimit);
            targetPos.z = -20f;
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, targetPos, smoothing);
        }

    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector2(leftLimit,topLimit),new Vector2(rightLimit,topLimit) );
        Gizmos.DrawLine(new Vector2(rightLimit,topLimit),new Vector2(rightLimit,bottomLimit) );
        Gizmos.DrawLine(new Vector2(rightLimit,bottomLimit),new Vector2(leftLimit,bottomLimit) );
        Gizmos.DrawLine(new Vector2(leftLimit,bottomLimit),new Vector2(leftLimit,topLimit) );
    }
}
