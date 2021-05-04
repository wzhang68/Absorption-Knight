using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sendToStart : MonoBehaviour
{
    [SerializeField]public Vector3 toPos;

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.name == "Teodoro_ZLVer"){
            other.gameObject.transform.SetPositionAndRotation(toPos, other.gameObject.transform.rotation);
        }
    }
}
