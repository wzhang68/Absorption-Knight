using System.Collections;
using UnityEngine;


public enum NormalEnemyState {
    Idle,
    Approaching,
    Attack,
    Hurt,
    Dead,
    StandOff,
}

public class NormalEnemy : Enemy{

    [Header("Normal Enemy Properties")]
    [SerializeField] protected NormalEnemyState m_normalEnemyState = NormalEnemyState.Idle;
    [SerializeField] protected bool m_grounded = false;
    [SerializeField] protected float m_attackRange = 2.0f;
    [SerializeField] protected float m_SleepRange = 8.0f;
    
    [Header("Normal Enemy Attack Properties")]
    [SerializeField] protected UnitAttackZone m_attackZone = null;
    [SerializeField] protected float m_attackCooldown = 2f;
    [SerializeField] protected float m_attackDelay = 2.0f;
    [SerializeField] protected int m_damagePerHit = 0;
    [SerializeField] protected float m_hitBackSpeed = 5.0f;

    protected EnemyGroundSensor m_groundSensor;
    protected Vector3 m_endOffset = new Vector3(0f, 1f);
    protected bool m_duringHitBack = false;
    protected float m_timeSinceAttack = 100.0f; //big number to avoid first cool down
    protected float m_timeSinceHitBack = 100.0f;
    protected bool m_duringAttack = false;


    protected override void Start(){
        base.Start();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<EnemyGroundSensor>();
        m_normalEnemyState = NormalEnemyState.Idle;
    }

    protected virtual void IdleAction() {
        if (m_target != null) {

            float distance = Vector2.Distance(transform.position, m_target.transform.position);
            if (distance > m_SleepRange) {
                return;
            }

            m_destination = m_target;
            m_normalEnemyState = NormalEnemyState.Approaching;
            if (m_path != null) {
                return;
            }
            if (!IsInvoking("UpdateEnemyPath")) {
                InvokeRepeating("UpdateEnemyPath", 0f, 0.5f);
            }
        }
    }


    protected virtual void ApproachingAction() {
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
                    Vector3 rightStart = transform.position + new Vector3(0.75f, 0f);
                    Vector3 rightEnd = rightStart + m_endOffset;
                    hitRight = Physics2D.Linecast(rightStart, rightEnd, mask);
                    Debug.DrawLine(rightStart, rightEnd, Color.green);
                    if (!hitRight) {
                        return;
                    }
                    m_rb2d.velocity = new Vector2(m_speed, m_rb2d.velocity.y);
                } else if (dir.x <= -0.25f) {
                    RaycastHit2D hitLeft;
                    Vector3 LeftStart = transform.position + new Vector3(-0.75f, 0f);
                    Vector3 LeftEnd = LeftStart + m_endOffset;
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

    protected virtual void AttackAction() {
        if (m_duringAttack) {
            Vector2 dir = new Vector2(transform.position.x >= m_target.transform.position.x ? -1f : 1f, 0f);
            m_rb2d.velocity = dir * 1f;
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, m_target.position);
        if (distanceToTarget > m_attackRange) {
            m_normalEnemyState = NormalEnemyState.Idle;
            return;
        }

        if (m_timeSinceAttack > m_attackCooldown) {
            StartCoroutine(DoAttack());
        }
    }

    protected virtual IEnumerator DoAttack() {
        if (m_attackZone == null) {
            Debug.LogError("Attack zone of " + gameObject.name + " is not assigned!");
            yield break;
        }

        m_normalEnemyState = NormalEnemyState.Attack;
        m_timeSinceAttack = 0.0f;
        m_attackZone.InitializeAttackZone(this, m_damagePerHit, 0.0f, 0.0f);

        yield return new WaitForSeconds(m_attackDelay);
        if(!m_dead){
            m_duringAttack = true;
            m_animator.SetTrigger("Attack");

            Vector2 dir;
            dir = new Vector2(transform.position.x >= m_target.transform.position.x ? -1f : 1f, 0f);
            m_rb2d.velocity = dir * 0.1f;
        }
        
    }

    public void AttackAnimFinish() {
        m_duringAttack = false;
        m_normalEnemyState = NormalEnemyState.Idle;
    }

    public override void OnAttackZoneHitUnit(Unit hitUnit, GameObject source, int damage) {
        base.OnAttackZoneHitUnit(hitUnit, source, damage);      //Hit(hitUnit, source, damage);
    }

    protected override void Hit(Unit hitUnit, GameObject source, int damage) {
        Vector2 hitBackDir = new Vector2(transform.position.x >= hitUnit.transform.position.x ? -1f : 1f, 1f);

        Debug.Log("Hitback speed: " + m_hitBackSpeed);
        hitUnit.OnHit(this.gameObject, source, damage, hitBackDir, m_hitBackSpeed);
    }

    protected override void OnHitBack(GameObject source, GameObject attacker, Vector2 hitBackDir, float hitBackSpeed) {
        base.OnHitBack(source, attacker, hitBackDir, hitBackSpeed);
        m_duringHitBack = true;
        m_timeSinceHitBack = 0.0f;
    }

    protected void UpdateAnimation() {
        //Set AirSpeed in animator
        m_animator.SetFloat("MoveSpeed", Mathf.Abs(m_rb2d.velocity.x));
    }

    protected void UpdateHitBackState() {
        if (m_duringHitBack) {
            m_timeSinceHitBack += Time.deltaTime;
            if (m_timeSinceHitBack > 2.0f || Mathf.Abs(m_rb2d.velocity.x) <= 0.01f) {
                m_duringHitBack = false;
            }
        }
    }

    protected void UpdateDir() {
        if (m_target != null) {
            if (m_target.position.x >= transform.position.x) {
                transform.localScale = new Vector3(1f, 1f, 1f);
            } else {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
    }

    protected void CheckGround() {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State()) {
            m_grounded = true;
            m_path = null;
            UpdateEnemyPath();
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State()) {
            m_grounded = false;
        }
    }

    protected void UpdateEnemyPath() {
        UpdatePath("CatKnightNavMesh");
    }

    protected override void Die() {
        base.Die();
        m_animator.SetTrigger("Die");
        m_rb2d.gravityScale = 1.0f;
        m_rb2d.mass = 9999f;
        m_dead = true;
        // m_rb2d.freezeRotation = true;
        // m_normalEnemyState = NormalEnemyState.Dead;
    }

    protected void DieAnimationFinish() {
        m_dead = true;
        // Destroy(gameObject, 0.1f);
    }

    protected void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, nextWaypointDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_SleepRange);
    }

}
