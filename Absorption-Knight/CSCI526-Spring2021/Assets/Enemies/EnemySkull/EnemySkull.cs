using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum EnemySkullState {
//     Idle,
//     Approaching,
//     Attack
// }

public class EnemySkull : NormalEnemy {

    protected override void Awake(){
        base.Awake();
        m_endOffset = new Vector3(0f, -1f);
    }

    protected override void Start(){
        base.Start();
        m_unitType = "skull";
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

    // [Header("Enemy Skull Properties")]
    // [SerializeField] EnemySkullState m_skullState = EnemySkullState.Idle;
    // [SerializeField] private bool m_grounded = false;
    // [SerializeField] float m_attackRange = 2.5f;
    // [SerializeField] float m_SleepRange = 8.0f;

    // [Header("Attack Properties")]
    // [SerializeField] UnitAttackZone m_attackZone = null;
    // [SerializeField] float m_attackCoolDown = 2.0f;
    // [SerializeField] float m_attackDelay = 1.0f;
    // [SerializeField] int m_damagePerHit = 5;
    // [SerializeField] float m_hitbackSpeed = 5.0f;

    // Sensor_CatKnight m_groundChecker;

    // bool m_duringHitback = false;
    // float m_timeSinceHitback = 100.0f;
    // private float m_timeSinceAttack = 100.0f;
    // bool m_duringAttack = false;

    // protected override void Start() {
    //     base.Start();
    //     m_groundChecker = transform.Find("GroundSensor").GetComponent<Sensor_CatKnight>();
    //     m_skullState = EnemySkullState.Idle;
    //     //InvokeRepeating("UpdateEnemySkullPath", 0.0f, 1.0f);
    // }

    // private void Update() {
    //     m_timeSinceAttack += Time.deltaTime;

    //     CheckGround();
    //     UpdateHitBackState();

    //     if (m_skullState == EnemySkullState.Idle) {
    //         IdleAction();
    //     } else if (m_skullState == EnemySkullState.Approaching) {
    //         ApproachingAction();
    //     } else if (m_skullState == EnemySkullState.Attack) {
    //         AttackAction();
    //     }
    //     UpdateAnimationInfo();
    // }
    
    // private void IdleAction() {
    //     if (m_target != null) {

    //         float distance = Vector2.Distance(transform.position, m_target.transform.position);
    //         if(distance > m_SleepRange) {
    //             return;
    //         }

    //         m_destination = m_target;
    //         m_skullState = EnemySkullState.Approaching;
    //         if (m_path != null) {
    //             return;
    //         }
    //         if(!IsInvoking("UpdateEnemySkullPath")) {
    //             InvokeRepeating("UpdateEnemySkullPath", 0.0f, 0.5f);
    //         }
    //     }
    // }

    // private void ApproachingAction() {
    //     float distanceToTarget = Vector2.Distance(transform.position, m_target.position);
    //     if (distanceToTarget <= m_attackRange) {
    //         m_skullState = EnemySkullState.Attack;
    //     }

    //     if (m_destination == null || m_path == null/* || m_currentWayPointIndex >= m_path.vectorPath.Count*/) {
    //         m_skullState = EnemySkullState.Idle;
    //         return;
    //     }
    //     if (!m_grounded || m_currentWayPointIndex >= m_path.vectorPath.Count) {
    //         return; 
    //     }

    //     float distance = Vector2.Distance(transform.position, m_target.transform.position);
    //     if (distance > m_SleepRange) {
    //         m_skullState = EnemySkullState.Idle;
    //         return;
    //     }

    //     UpdateDir();

    //     LayerMask mask = ~(1 << LayerMask.NameToLayer("TriggerCollider"));
    //     //LayerMask mask = 0 << 11;

    //     Debug.DrawLine(m_path.vectorPath[m_currentWayPointIndex], m_path.vectorPath[m_currentWayPointIndex] + new Vector3(0, 1, 0), Color.red);
    //     Vector2 dir = ((Vector2)m_path.vectorPath[m_currentWayPointIndex] - m_rb2d.position).normalized;
    //     float distanceToNextWaypoint = Vector2.Distance(m_rb2d.position, m_path.vectorPath[m_currentWayPointIndex]);
    //     if (distanceToNextWaypoint < nextWaypointDistance) {
    //         m_currentWayPointIndex++;
    //     } else {
    //         if (!m_duringHitback && !m_duringAttack) {
    //             if (dir.x >= 0.25f /*&& m_rightWallSensor.State()*/) {
    //                 RaycastHit2D hitRight;
    //                 Vector3 rightStart = transform.position + new Vector3(0.75f, 0f);
    //                 Vector3 rightEnd = rightStart + new Vector3(0f, -1f);
    //                 hitRight = Physics2D.Linecast(rightStart, rightEnd, mask);
    //                 Debug.DrawLine(rightStart, rightEnd, Color.green);
    //                 if (!hitRight) {
    //                     return;
    //                 }
    //                 m_rb2d.velocity = new Vector2(m_speed, m_rb2d.velocity.y);
    //             } else if (dir.x <= -0.25f /*&& m_leftWallSensor.State()*/) {
    //                 RaycastHit2D hitLeft;
    //                 Vector3 LeftStart = transform.position + new Vector3(-0.75f, 0f);
    //                 Vector3 LeftEnd = LeftStart + new Vector3(0f, -1f);
    //                 hitLeft = Physics2D.Linecast(LeftStart, LeftEnd, mask);
    //                 Debug.DrawLine(LeftStart, LeftEnd, Color.green);
    //                 if (!hitLeft) {
    //                     return;
    //                 }
    //                 m_rb2d.velocity = new Vector2(-m_speed, m_rb2d.velocity.y);
    //             }
    //         }
    //     }
    // }

    // private void AttackAction() {
    //     if (m_duringAttack) {
    //         Vector2 dir = new Vector2(transform.position.x >= m_target.transform.position.x ? -1f : 1f, 0f);
    //         m_rb2d.velocity = dir * 1f;
    //         return;
    //     }

    //     float distanceToTarget = Vector2.Distance(transform.position, m_target.position);
    //     if (distanceToTarget > m_attackRange) {
    //         m_skullState = EnemySkullState.Idle;
    //         return;
    //     }

    //     if(m_timeSinceAttack > m_attackCoolDown) {
    //         StartCoroutine(DoAttack());
    //     }

    // }

    // private IEnumerator DoAttack() {
    //     if(m_attackZone == null) {
    //         Debug.LogError("Attack zone of " + gameObject.name + " is not assigned!");
    //         yield break;
    //     }

    //     m_skullState = EnemySkullState.Attack;
    //     m_timeSinceAttack = 0.0f;
    //     m_attackZone.InitializeAttackZone(this, m_damagePerHit, 0.0f, 0.0f);

    //     yield return new WaitForSeconds(m_attackDelay);

    //     m_duringAttack = true;
    //     m_animator.SetTrigger("Attack");

    //     Vector2 dir;
    //     dir = new Vector2(transform.position.x >= m_target.transform.position.x ? -1f : 1f, 0f);
    //     m_rb2d.velocity = dir * 0.1f;
    // }

    // public void AttackAnimFinish() {
    //     m_duringAttack = false;
    //     m_skullState = EnemySkullState.Idle;
    // }

    // public override void OnAttackZoneHitUnit(Unit hitUnit, GameObject source, int damage) {
    //     base.OnAttackZoneHitUnit(hitUnit, source, damage);  //Hit(hitUnit, source, damage);
    // }
    

    // protected override void Hit(Unit hitUnit, GameObject source, int damage) {
    //     Vector2 hitBackDir;
    //     //hitBackDir = (hitUnit.transform.position - transform.position).normalized;
    //     hitBackDir = new Vector2(transform.position.x > hitUnit.transform.position.x ? -1f : 1f, 1f);

    //     //Debug.Log("Hitback speed: " + m_currentAtkHitbackSpeed);
    //     hitUnit.OnHit(this.gameObject, source, damage, hitBackDir, m_hitbackSpeed);
    // }

    // protected override void OnHitBack(GameObject source, GameObject attacker, Vector2 dir, float hitBackSpeed) {
    //     base.OnHitBack(source, attacker, dir, hitBackSpeed);
    //     m_duringHitback = true;
    //     m_timeSinceHitback = 0.0f;
    // }

    // void CheckGround() {
    //     //Check if character just landed on the ground
    //     if (!m_grounded && m_groundChecker.State()) {
    //         m_grounded = true;
    //         m_path = null;
    //         UpdateEnemySkullPath();
    //     }

    //     //Check if character just started falling
    //     if (m_grounded && !m_groundChecker.State()) {
    //         m_grounded = false;
    //     }
    // }

    // void UpdateEnemySkullPath() {
    //     if(m_grounded && m_skullState == EnemySkullState.Approaching) {
    //         //share same nav mesh
    //         UpdatePath("CatKnightNavMesh");
    //     }
    // }

    // void UpdateDir() {
    //     if (m_target != null) {
    //         if (m_target.position.x >= transform.position.x) {
    //             //m_graphics.localScale = new Vector3(1f, 1f, 1f);
    //             transform.localScale = new Vector3(1f, 1f, 1f);
    //         } else {
    //             //m_graphics.localScale = new Vector3(-1f, 1f, 1f);
    //             transform.localScale = new Vector3(-1f, 1f, 1f);
    //         }
    //     }
    // }

    // void UpdateAnimationInfo() {
    //     m_animator.SetFloat("AbsHorMoveSpeed", Mathf.Abs(m_rb2d.velocity.x));
    // }

    // private void UpdateHitBackState() {
    //     if (m_duringHitback) {
    //         m_timeSinceHitback += Time.deltaTime;
    //         if (m_timeSinceHitback > 2.0f || Mathf.Abs(m_rb2d.velocity.x) <= 0.01f) {
    //             m_duringHitback = false;
    //         }
    //     }
    // }

    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireSphere(transform.position, nextWaypointDistance);
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, m_attackRange);
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, m_SleepRange);
    // }
}
