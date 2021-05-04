using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class EnemyWarrior : NormalEnemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        m_unitType = "EnemyWarrior";
    }

    // Update is called once per frame
    void Update()
    {
        if (m_dead) {
            return;
        }
        m_timeSinceAttack += Time.deltaTime;

        CheckGround();
        UpdateHitBackState();
        if (m_normalEnemyState == NormalEnemyState.Idle) {
            IdleAction();
        } else if (m_normalEnemyState == NormalEnemyState.Approaching) {
            ApproachingAction();
        } else if (m_normalEnemyState == NormalEnemyState.Attack) {
            AttackAction();
        }
        UpdateAnimation();
    }

    protected override void Die() {
        base.Die();
        Debug.Log("die in warrior");
        // base.m_animator.ResetTrigger("Attack");
        m_animator.SetBool("Die", true);
        m_rb2d.gravityScale = 1.0f;
        m_rb2d.mass = 9999f;
        m_dead = true;
    }

    void afterDie(){
        base.m_animator.speed = 0 ;
    }
}
