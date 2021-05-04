using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoblinGFXManager : MonoBehaviour{

    [SerializeField] EnemyGoblin m_enemyGoblin = null;


    // Start is called before the first frame update
    private void Start() {
        if(m_enemyGoblin == null){
            Debug.Log("EnemyGoblin game object is not assigned!");
            this.enabled = false;
        }
        // m_enemyGoblin.SetEnemyGoblinGFXManager();
    }

   
}
