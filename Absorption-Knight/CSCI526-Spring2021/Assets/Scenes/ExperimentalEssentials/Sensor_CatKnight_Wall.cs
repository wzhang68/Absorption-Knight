using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sensor_CatKnight_Wall : MonoBehaviour
{
    private int OntheWall = 0;
    private Collider2D m_collider;
    //private EdgeCollider2D m_collider;
    //private BoxCollider2D m_collider;

    private void Awake()
    {
        if(!this.GetComponent<EdgeCollider2D>()) {
            m_collider = this.GetComponent<BoxCollider2D>();
        } else {
            m_collider = this.GetComponent<EdgeCollider2D>();
        }
            
    }

    public bool State(){
        //Debug.Log("wall" + OntheWall);
        return OntheWall == 0;
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "teleport" || other.gameObject.tag=="ladder"|| other.gameObject.tag=="purchasable" ||other.gameObject.tag=="Enemy" ||other.gameObject.tag=="MovePlatform"||other.gameObject.tag=="tutorial"||other.gameObject.tag =="End")
        {
            m_collider.enabled = false;
            return;
        }
        if (ShouldIgnoreCollision(other)) {
            return;
        }
        OntheWall++;
        //Debug.Log("now on wall");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!m_collider.isActiveAndEnabled)
        {
            m_collider.enabled = true;
            //if(other.gameObject.tag == "teleport" || other.gameObject.tag=="ladder"|| other.gameObject.tag=="purchasable" ||other.gameObject.tag=="Enemy" ||other.gameObject.tag=="MovePlatform")
            OntheWall++;
        }
        if (ShouldIgnoreCollision(other)) {
            return;
        }
        OntheWall --;
    }

    public void OutputDebug(String header) {
        Debug.Log(header + "OntheWall: " + OntheWall);
    }

    bool ShouldIgnoreCollision(Collider2D collision) {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("TriggerCollider")) ||
            collision.gameObject.GetComponent<UnitAttackZone>() != null ||
            collision.gameObject.CompareTag("EnemyLurker")) {
            return true;
        } else {
            return false;
        }
    }
    
}
