using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhantomKnight : NormalEnemy{

    [Header("Enemy Phantom Knight Attack Properties")]
    [SerializeField] protected UnitAttackZone m_attackZone2;
    [SerializeField] protected UnitAttackZone m_attackZone3;
    [SerializeField] protected int m_damage2PerHit = 0;
    [SerializeField] protected int m_damage3PerHit = 0;


    protected override void Awake(){
        base.Awake();
        m_endOffset = new Vector3(0f, -2f);
    }

    protected override void Start(){
        base.Start();
        m_unitType = "EnemyPhantomKnight";
    }

    protected void Update(){
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

    protected override IEnumerator DoAttack(){
        if (m_attackZone == null) {
            Debug.LogError("Attack zone 1 of " + gameObject.name + " is not assigned!");
            yield break;
        }
        if (m_attackZone2 == null) {
            Debug.LogError("Attack zone 2 of " + gameObject.name + " is not assigned!");
            yield break;
        }
        if (m_attackZone3 == null) {
            Debug.LogError("Attack zone 3 of " + gameObject.name + " is not assigned!");
            yield break;
        }

        m_normalEnemyState = NormalEnemyState.Attack;
        m_timeSinceAttack = 0.0f;
        m_attackZone.InitializeAttackZone(this, m_damagePerHit, 0.0f, 0.0f);
        m_attackZone2.InitializeAttackZone(this, m_damage2PerHit, 0.0f, 0.0f);
        m_attackZone3.InitializeAttackZone(this, m_damage3PerHit, 0.0f, 0.0f);
        yield return new WaitForSeconds(m_attackDelay);
        if(!m_dead){
            m_duringAttack = true;
            m_animator.SetTrigger("Attack");

            Vector2 dir;
            dir = new Vector2(transform.position.x >= m_target.transform.position.x ? -1f : 1f, 0f);
            m_rb2d.velocity = dir * 0.1f;
        }
    }
}
