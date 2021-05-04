using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReaperMoveArea : MonoBehaviour
{
    [SerializeField] private float m_width;
    [SerializeField] private float m_height;
    [SerializeField] private Transform m_currentDestionation;

    public Transform GetRandomTransform() {
        float randomx = Random.Range(transform.position.x - m_width / 2, transform.position.x + m_width / 2);
        float randomy = Random.Range(transform.position.y - m_height / 2, transform.position.y + m_height / 2);
        m_currentDestionation.position = new Vector3(randomx, randomy, 0);
        return m_currentDestionation;
    }

    public bool InsideArea(Vector2 position2D) {
        if((transform.position.x - m_width / 2) <= position2D.x && 
            position2D.x <= (transform.position.x + m_width / 2) &&

            (transform.position.y - m_height / 2) <= position2D.y &&
            position2D.y <= (transform.position.y + m_height / 2)) {

            return true;
        } else {
            return false;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector3 center = transform.position;
        Gizmos.DrawLine(
            new Vector3(center.x + m_width / 2, center.y - m_height / 2, 0f),
            new Vector3(center.x + m_width / 2, center.y + m_height / 2, 0f)
        );

        Gizmos.DrawLine(
            new Vector3(center.x - m_width / 2, center.y - m_height / 2, 0f),
            new Vector3(center.x - m_width / 2, center.y + m_height / 2, 0f)
        );

        Gizmos.DrawLine(
            new Vector3(center.x + m_width / 2, center.y - m_height / 2, 0f),
            new Vector3(center.x - m_width / 2, center.y - m_height / 2, 0f)
        );

        Gizmos.DrawLine(
            new Vector3(center.x + m_width / 2, center.y + m_height / 2, 0f),
            new Vector3(center.x - m_width / 2, center.y + m_height / 2, 0f)
        );
    }
}
