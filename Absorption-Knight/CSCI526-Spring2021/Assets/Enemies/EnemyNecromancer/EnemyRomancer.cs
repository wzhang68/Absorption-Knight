using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Pathfinding;

// [RequireComponent(typeof(Seeker))]
public class EnemyRomancer : Enemy
{

    [Header("EnemySorcerer EnemyRomancer")]
    [SerializeField] private float m_attackRange = 10.0f;
    [SerializeField] private float m_attackInterval = 3.0f;
    [SerializeField] private int m_damagePerBullet = 10;
    [SerializeField] private float m_bulletDestoryDelay = 1.0f;
    [SerializeField] private float m_bulletSpeed = 2.5f;
    [SerializeField] private float m_bulletHitbackSpeed = 2.0f;
    [SerializeField] private Transform m_bulletSpawnPos = null;
    [SerializeField] private EnemyProjectile m_bulletPrefab = null;

    private float m_timeSinceAttack = 0.0f;

    // Update is called once per frame
    void Update()
    {
        m_timeSinceAttack += Time.deltaTime;

        if(m_target == null) {
            return;
        }

        transform.localScale = new Vector3(m_target.transform.position.x <= transform.position.x ? 1f : -1f, 1f, 1f);

        float dis = Vector2.Distance(transform.position, m_target.transform.position);
        if(dis <= m_attackRange && m_timeSinceAttack >= m_attackInterval) {
            m_animator.SetTrigger("Attack");
            m_timeSinceAttack = 0.0f;
        }
    }

    private void SpawnBullet() {
        Debug.Log("spawn");
        EnemyProjectile bullet = Instantiate(m_bulletPrefab, m_bulletSpawnPos.position, m_bulletSpawnPos.rotation);
        bullet.InitializeEnemyBullet(this, m_damagePerBullet, m_target, m_bulletSpeed, m_bulletDestoryDelay);
    }

    protected override void Hit(Unit target, GameObject source, int damage) {
        Vector3 hitBackDir = target.transform.position - source.transform.position;

        target.OnHit(this.gameObject, source, damage, hitBackDir, m_bulletHitbackSpeed);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attackRange);
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
}
