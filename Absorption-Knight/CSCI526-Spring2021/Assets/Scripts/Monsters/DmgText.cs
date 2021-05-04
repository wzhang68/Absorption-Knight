using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DmgText : MonoBehaviour
{
    private TextMeshPro textMesh;
    // Start is called before the first frame update
    // private void Awake()
    // {
    //     textMesh = transform.GetComponent<TextMeshPro>();
    //     
    // }
    //
    // public void Setup(int dmg)
    // {
    //     textMesh.SetText(dmg.ToString());
    // }

    public void DestoryOnAnimationEnd()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        UnityEngine.Object.Destroy(parent);
    }
}
