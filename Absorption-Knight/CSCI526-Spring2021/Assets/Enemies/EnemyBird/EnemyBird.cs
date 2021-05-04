using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//[RequireComponent(typeof(MonsterHPGauge))]
[RequireComponent(typeof(Seeker))]
public class EnemyBird : Enemy {

    [Header("EnemyBird Properties")]
    [SerializeField] private float m_hitBackSpeed = 6f;
    [SerializeField] private float m_selfHitBackSpeed = 5f;
    [SerializeField] protected int m_damagePerHit = 0;

    protected override void Start() {
        base.Start();
        InvokeRepeating("UpdateBirdEnemyPath", 0f, 0.5f);
    }

    private void FixedUpdate() {
        if (m_dead) { return; }
        UpdateTransform();
    }

    protected override void Hit(Unit hitUnit, GameObject source, int damage) {
        //self hit back
        Vector3 hitBackDir = hitUnit.transform.position - transform.position;
        m_rb2d.velocity = -hitBackDir * m_selfHitBackSpeed;

        hitUnit.OnHit(this.gameObject, source, damage, hitBackDir, m_hitBackSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit != null) {
            Hit(unit, this.gameObject, m_damagePerHit);
        }
    }

    protected override void OnHitBack(GameObject source, GameObject attacker, Vector2 dir, float hitBackSpeed) {
        base.OnHitBack(source, attacker, dir, hitBackSpeed);
    }

    private void UpdateTransform() {
        if (m_destination == null || m_path == null || m_currentWayPointIndex >= m_path.vectorPath.Count) {
            return;
        }

        Debug.DrawLine(m_path.vectorPath[m_currentWayPointIndex], m_path.vectorPath[m_currentWayPointIndex] + new Vector3(0, 1, 0), Color.red);

        Vector2 dir = ((Vector2)m_path.vectorPath[m_currentWayPointIndex] - m_rb2d.position).normalized;
        Vector2 force = dir * m_speed * Time.deltaTime;

        //m_rb2d.velocity = force;

        m_rb2d.AddForce(force);
        m_rb2d.velocity = Vector2.ClampMagnitude(m_rb2d.velocity, m_speed);
        //Debug.Log(m_rb2d.velocity);

        float distance = Vector2.Distance(m_rb2d.position, m_path.vectorPath[m_currentWayPointIndex]);
        if (distance < nextWaypointDistance) {
            m_currentWayPointIndex++;
        }

        if (force.x >= 0.01f) {
            m_graphics.localScale = new Vector3(3f, 3f, 3f);
            //transform.localScale = new Vector3(1f, 1f, 1f);
        } else if (force.x <= -0.01f) {
            m_graphics.localScale = new Vector3(-3f, 3f, 3f);
            //transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void UpdateBirdEnemyPath() {
        UpdatePath("BirdNavMesh");
    }

    protected override void Die() {
        base.Die();
        m_rb2d.gravityScale = 1.0f;
        m_rb2d.mass = 9999f;
        m_dead = true;
        Destroy(gameObject, 3f);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, nextWaypointDistance);
    }
}
