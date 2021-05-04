using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIceGolem : SpecialEnemy{

    protected override void Awake() {
        // initialize enemy properties here
        base.Awake();
        m_endOffset = new Vector3(0f, -2f);
    }
    
    protected override void Start(){
        base.Start();
        m_unitType = "EnemyIceGolem";
    }

    protected void Update() {
        if(m_dead) {
            return;
        }
        m_timeSinceAttack += Time.deltaTime;
        m_timeSinceStandOff += Time.deltaTime;

        CheckGround();
        UpdateHitBackState();
        if (m_normalEnemyState == NormalEnemyState.Idle) {
            IdleAction();
        } else if (m_normalEnemyState == NormalEnemyState.Approaching) {
            ApproachingAction();
        } else if (m_normalEnemyState == NormalEnemyState.Attack) {
            AttackAction();
        } else if(m_normalEnemyState == NormalEnemyState.StandOff){
            StandOffAction();
        }
        UpdateAnimation();
    }
}