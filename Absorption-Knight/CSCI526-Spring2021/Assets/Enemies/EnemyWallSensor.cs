using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyWallSensor : MonoBehaviour
{
    private int OntheWall = 0;
    private EdgeCollider2D m_collider;

    private void Awake()
    {
        m_collider = this.GetComponent<EdgeCollider2D>();
    }

    public bool State(){
        //Debug.Log("wall" + OntheWall);
        return OntheWall == 0;
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "teleport" || other.gameObject.tag=="ladder"|| other.gameObject.tag=="purchasable" )
        {
            m_collider.enabled = false;
            return;
        }
        if (other.gameObject.GetComponent<UnitAttackZone>() != null ||
            other.gameObject.CompareTag("EnemyLurker")) {
            return;
        }
        OntheWall++;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!m_collider.isActiveAndEnabled)
        {
            m_collider.enabled = true;
            OntheWall++;
        }
        if (other.gameObject.GetComponent<UnitAttackZone>() != null ||
            other.gameObject.CompareTag("EnemyLurker")) {
            return;
        }
        OntheWall --;
    }

    public void OutputDebug(string header) {
        Debug.Log(header + "OntheWall: " + OntheWall);
    }
    
}
