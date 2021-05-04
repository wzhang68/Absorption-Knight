using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;

// public enum EnemyGoblinState {
//     Idle,
//     Approaching,
//     Attack,
//     Hurt,
//     Dead,
// }

[RequireComponent(typeof(Seeker))]
public class EnemyGoblin : NormalEnemy {

    protected override void Awake(){
        base.Awake();
        m_endOffset = new Vector3(0f, -1f);
    }

    protected override void Start(){
        base.Start();
        // m_unitType = "goblin";
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

    // [Header("Enemy Goblin Properties")]
    // [SerializeField] private EnemyGoblinState m_enemyGoblinState = EnemyGoblinState.Idle;
    // [SerializeField] private bool m_grounded = false;
    // [SerializeField] float m_attackRange = 2.0f;
    // [SerializeField] float m_SleepRange = 8.0f;

    // [Header("Enemy Goblin Attack Properties")]
    // [SerializeField] private UnitAttackZone m_attackZone = null;
    // [SerializeField] private float m_attackCooldown = 2.5f;
    // [SerializeField] private float m_attackDelay = 1.0f;
    // [SerializeField] private int m_damagePerHit = 0;
    // [SerializeField] private float m_hitBackSpeed = 5.0f;

    // private EnemyGroundSensor m_groundSensor;
    // private EnemyWallSensor m_leftWallSensor;
    // private EnemyWallSensor m_rightWallSensor;

    // bool m_duringHitBack = false;
    // private float m_timeSinceAttack = 100.0f; //big number to avoid first cool down
    // private float m_timeSinceHitBack = 100.0f;
    // bool m_duringAttack = false;


    // // protected override void Awake() {
    // //     base.Awake();

    // //     // m_animator = m_graphics.GetComponent<Animator>();
    // //     // m_leftWallSensor = transform.Find("PlayerLeftWallSensor").GetComponent<EnemyWallSensor>();
    // //     // m_rightWallSensor = transform.Find("PlayerRightWallSensor").GetComponent<EnemyWallSensor>();
    // // }

    // protected override void Start() {
    //     base.Start();
    //     // m_destination = m_target;
    //     m_unitType = "goblin";
    //     // m_skillType = "AddMaxHP";
    //     m_groundSensor = transform.Find("GroundSensor").GetComponent<EnemyGroundSensor>();
    //     m_enemyGoblinState = EnemyGoblinState.Idle;
    // }

    // private void Update() {
    //     m_timeSinceAttack += Time.deltaTime;

    //     CheckGround();
    //     UpdateHitBackState();
    //     if (m_enemyGoblinState == EnemyGoblinState.Idle) {
    //         IdleAction();
    //     } else if (m_enemyGoblinState == EnemyGoblinState.Approaching) {
    //         ApproachingAction();
    //     } else if (m_enemyGoblinState == EnemyGoblinState.Attack) {
    //         AttackAction();
    //     }
    //     UpdateAnimation();
    // }

    // private void IdleAction() {
    //     if (m_target != null) {

    //         float distance = Vector2.Distance(transform.position, m_target.transform.position);
    //         if (distance > m_SleepRange) {
    //             return;
    //         }

    //         m_destination = m_target;
    //         m_enemyGoblinState = EnemyGoblinState.Approaching;
    //         if (m_path != null) {
    //             return;
    //         }
    //         if (!IsInvoking("UpdateGoblinPath")) {
    //             InvokeRepeating("UpdateGoblinPath", 0f, 0.5f);
    //         }
    //     }
    // }

    // private void ApproachingAction() {
    //     float distanceToTarget = Vector2.Distance(transform.position, m_target.position);
    //     if (distanceToTarget <= m_attackRange) {
    //         m_enemyGoblinState = EnemyGoblinState.Attack;
    //     }

    //     if (m_destination == null || m_path == null/* || m_currentWayPointIndex >= m_path.vectorPath.Count*/) {
    //         m_enemyGoblinState = EnemyGoblinState.Idle;
    //         return;
    //     }

    //     if (!m_grounded || m_currentWayPointIndex >= m_path.vectorPath.Count) {
    //         return;
    //     }

    //     float distance = Vector2.Distance(transform.position, m_target.transform.position);
    //     if (distance > m_SleepRange) {
    //         m_enemyGoblinState = EnemyGoblinState.Idle;
    //     }

    //     UpdateDir();

    //     LayerMask mask = ~(1 << LayerMask.NameToLayer("TriggerCollider"));

    //     Debug.DrawLine(m_path.vectorPath[m_currentWayPointIndex], m_path.vectorPath[m_currentWayPointIndex] + new Vector3(0, 1, 0), Color.red);
    //     Vector2 dir = ((Vector2)m_path.vectorPath[m_currentWayPointIndex] - m_rb2d.position).normalized;
    //     float distanceToNextWaypoint = Vector2.Distance(m_rb2d.position, m_path.vectorPath[m_currentWayPointIndex]);
    //     if (distanceToNextWaypoint < nextWaypointDistance) {
    //         m_currentWayPointIndex++;
    //     } else {
    //         if (!m_duringHitBack && !m_duringAttack) {
    //             if (dir.x >= 0.25f) {
    //                 RaycastHit2D hitRight;
    //                 Vector3 rightStart = transform.position + new Vector3(0.5f, 0f);
    //                 Vector3 rightEnd = rightStart + new Vector3(0f, -1f);
    //                 hitRight = Physics2D.Linecast(rightStart, rightEnd, mask);
    //                 Debug.DrawLine(rightStart, rightEnd, Color.green);
    //                 if (!hitRight) {
    //                     if (m_rb2d.velocity.sqrMagnitude > 4) {
    //                         m_rb2d.velocity = m_rb2d.velocity.normalized * 1.5f;
    //                     }
    //                     return;
    //                 }
    //                 m_rb2d.velocity = new Vector2(m_speed, m_rb2d.velocity.y);
    //             } else if (dir.x <= -0.25f) {
    //                 RaycastHit2D hitLeft;
    //                 Vector3 LeftStart = transform.position + new Vector3(-0.5f, 0f);
    //                 Vector3 LeftEnd = LeftStart + new Vector3(0f, -1f);
    //                 hitLeft = Physics2D.Linecast(LeftStart, LeftEnd, mask);
    //                 Debug.DrawLine(LeftStart, LeftEnd, Color.green);
    //                 if (!hitLeft) {
    //                     if (m_rb2d.velocity.sqrMagnitude > 4) {
    //                         m_rb2d.velocity = m_rb2d.velocity.normalized * 1.5f;
    //                     }
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
    //         m_enemyGoblinState = EnemyGoblinState.Idle;
    //         return;
    //     }

    //     if (m_timeSinceAttack > m_attackCooldown) {
    //         StartCoroutine(DoAttack());
    //     }
    // }

    // private IEnumerator DoAttack() {
    //     if (m_attackZone == null) {
    //         Debug.LogError("Attack zone of " + gameObject.name + " is not assigned!");
    //         yield break;
    //     }

    //     m_enemyGoblinState = EnemyGoblinState.Attack;
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
    //     m_enemyGoblinState = EnemyGoblinState.Idle;
    // }

    // public override void OnAttackZoneHitUnit(Unit hitUnit, GameObject source, int damage) {
    //     base.OnAttackZoneHitUnit(hitUnit, source, damage);      //Hit(hitUnit, source, damage);
    // }

    // protected override void Hit(Unit hitUnit, GameObject source, int damage) {
    //     Vector2 hitBackDir = new Vector2(transform.position.x >= hitUnit.transform.position.x ? -1f : 1f, 1f);

    //     //Debug.Log("Hitback speed: " + m_currentAtkHitbackSpeed);
    //     hitUnit.OnHit(this.gameObject, source, damage, hitBackDir, m_hitBackSpeed);
    // }

    // protected override void OnHitBack(GameObject source, GameObject attacker, Vector2 hitBackDir, float hitBackSpeed) {
    //     base.OnHitBack(source, attacker, hitBackDir, hitBackSpeed);
    //     m_duringHitBack = true;
    //     m_timeSinceHitBack = 0.0f;
    // }

    // private void UpdateAnimation() {
    //     //Set AirSpeed in animator
    //     m_animator.SetFloat("MoveSpeed", Mathf.Abs(m_rb2d.velocity.x));
    // }

    // private void UpdateHitBackState() {
    //     if (m_duringHitBack) {
    //         m_timeSinceHitBack += Time.deltaTime;
    //         if (m_timeSinceHitBack > 2.0f || Mathf.Abs(m_rb2d.velocity.x) <= 0.01f) {
    //             m_duringHitBack = false;
    //         }
    //     }
    // }

    // private void UpdateDir() {
    //     if (m_target != null) {
    //         if (m_target.position.x >= transform.position.x) {
    //             //m_graphics.localScale = new Vector3(1f, 1f, 1f);
    //             transform.localScale = new Vector3(1f, 1f, 1f);
    //             // m_rightWallSensor.transform.localScale = new Vector3(1f, 1f, 1f);
    //             // m_leftWallSensor.transform.localScale = new Vector3(1f, 1f, 1f);
    //         } else {
    //             //m_graphics.localScale = new Vector3(-1f, 1f, 1f);
    //             transform.localScale = new Vector3(-1f, 1f, 1f);
    //             // m_rightWallSensor.transform.localScale = new Vector3(-1f, 1f, 1f);
    //             // m_leftWallSensor.transform.localScale = new Vector3(-1f, 1f, 1f);
    //         }
    //     }
    // }

    // private void UpdateGoblinPath() {
    //     UpdatePath("CatKnightNavMesh");
    // }

    // void CheckGround() {
    //     //Check if character just landed on the ground
    //     if (!m_grounded && m_groundSensor.State()) {
    //         m_grounded = true;
    //         m_path = null;
    //         UpdateGoblinPath();
    //     }

    //     //Check if character just started falling
    //     if (m_grounded && !m_groundSensor.State()) {
    //         m_grounded = false;
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