using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


[RequireComponent(typeof(Seeker))]
public class EnemyGhoul : NormalEnemy
{
    // protected override void Awake() {
    //     // initialize enemy properties here
    //     base.Awake();
        
    // }
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // m_animator = m_graphics.GetComponent<Animator>();

        m_unitType = "EnemyGhoul";
    }

    // Update is called once per frame
    void Update()
    {
        if (m_dead) {
            // Debug.Log("return");
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
        Debug.Log("die in ghoul");
        // base.m_animator.ResetTrigger("Attack");
        m_animator.SetBool("Die", true);
        m_rb2d.gravityScale = 1.0f;
        m_rb2d.mass = 9999f;
        m_dead = true;
    }

    void afterDie(){
        base.m_animator.speed = 0 ;
    }

    protected override void ApproachingAction() {
        float distanceToTarget = Vector2.Distance(transform.position, m_target.position);
        if (distanceToTarget <= m_attackRange) {
            m_normalEnemyState = NormalEnemyState.Attack;
        }

        if (m_destination == null || m_path == null/* || m_currentWayPointIndex >= m_path.vectorPath.Count*/) {
            m_normalEnemyState = NormalEnemyState.Idle;
            return;
        }

        if (!m_grounded || m_currentWayPointIndex >= m_path.vectorPath.Count) {
            return;
        }

        float distance = Vector2.Distance(transform.position, m_target.transform.position);
        if (distance > m_SleepRange) {
            m_normalEnemyState = NormalEnemyState.Idle;
        }

        UpdateDir();

        LayerMask mask = ~(1 << LayerMask.NameToLayer("TriggerCollider"));

        Debug.DrawLine(m_path.vectorPath[m_currentWayPointIndex], m_path.vectorPath[m_currentWayPointIndex] + new Vector3(0, 1, 0), Color.red);
        Vector2 dir = ((Vector2)m_path.vectorPath[m_currentWayPointIndex] - m_rb2d.position).normalized;
        float distanceToNextWaypoint = Vector2.Distance(m_rb2d.position, m_path.vectorPath[m_currentWayPointIndex]);
        if (distanceToNextWaypoint < nextWaypointDistance) {
            m_currentWayPointIndex++;
        } else {
            if (!m_duringHitBack && !m_duringAttack) {
                if (dir.x >= 0.25f) {
                    RaycastHit2D hitRight;
                    Vector3 rightStart = transform.position + new Vector3(1.25f, 0f);
                    Vector3 rightEnd = rightStart + new Vector3(0f, -1f);
                    hitRight = Physics2D.Linecast(rightStart, rightEnd, mask);
                    Debug.DrawLine(rightStart, rightEnd, Color.green);
                    if (!hitRight) {
                        return;
                    }
                    m_rb2d.velocity = new Vector2(m_speed, m_rb2d.velocity.y);
                } else if (dir.x <= -0.25f) {
                    RaycastHit2D hitLeft;
                    Vector3 LeftStart = transform.position + new Vector3(-1.25f, 0f);
                    Vector3 LeftEnd = LeftStart + new Vector3(0f, -1f);
                    hitLeft = Physics2D.Linecast(LeftStart, LeftEnd, mask);
                    Debug.DrawLine(LeftStart, LeftEnd, Color.green);
                    if (!hitLeft) {
                        return;
                    }
                    m_rb2d.velocity = new Vector2(-m_speed, m_rb2d.velocity.y);
                }
            }
        }
    }
}
