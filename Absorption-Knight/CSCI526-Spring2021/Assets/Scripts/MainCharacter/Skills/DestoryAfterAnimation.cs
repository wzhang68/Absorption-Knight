using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryAfterAnimation : MonoBehaviour
{
    public void DestoryOnAnimationEnd()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        Destroy(parent);
    }
    
    public void DestoryOnAnimationEndWOParent()
    {
        Destroy(gameObject);
    }
}
