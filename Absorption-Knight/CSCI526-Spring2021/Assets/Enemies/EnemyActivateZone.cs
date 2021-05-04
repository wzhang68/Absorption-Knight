using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivateZone : MonoBehaviour {

    private Enemy m_linkedEnemy = null;

    private void Start() {
        //if(linkedEnemy == null) {
        //    Debug.LogError("linkedEnemy of " + gameObject.name + "is not assigned!");
        //}
        if (!gameObject.layer.Equals(LayerMask.NameToLayer("TriggerCollider"))) {
            Debug.LogError(gameObject.name + "'s layer is not set correctly");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerUnit player = collision.gameObject.GetComponent<PlayerUnit>();
        if (m_linkedEnemy != null && player != null) {
            m_linkedEnemy.OnPlayerEnterActivateZone(player, this);
        }
    }

    public void SetLinkedEnemy(Enemy enemy) {
        m_linkedEnemy = enemy;
    }

}
