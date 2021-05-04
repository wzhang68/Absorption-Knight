using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RestrictionArea2D {
    public Vector2 centerOffset;
    public float length;
    public float width;
};

[System.Serializable]
public struct ImpMuzzle {
    public Transform transform;
    private bool isNormalized;
    [SerializeField] private Vector2 dirInLocal;
    public Vector2 GetNormalizedDirInWorld() {
        if (isNormalized) {
            return transform.TransformVector(dirInLocal);
        } else {
            return transform.TransformVector(dirInLocal).normalized;
        }
    }
    public void NormalizeDir() {
        dirInLocal.Normalize();
        isNormalized = true;
    }
}

public class EnemyImp : Enemy {

    [Header("EnemyImp Properties")]
    [SerializeField] RestrictionArea2D[] m_restrictionAreas;
    [SerializeField] private int m_targetAreaIndex = 1;
    [SerializeField] float m_moveForceRatio = 4.0f;
    [SerializeField] float m_maxMoveForce = 4.0f;

    [Header("EnemyImp Attack properties")]
    [SerializeField] ImpMuzzle[] m_muzzles = null;
    [SerializeField] float m_attackCoolDown = 4.0f;
    [SerializeField] EnemyImpBullet m_bulletPrefab = null;
    [SerializeField] int m_bulletDamage = 10;
    [SerializeField] float m_bulletSpeed = 5.0f;
    [SerializeField] float m_bulletDestroyDelay = 6.0f;
    [SerializeField] float m_bulletHitbackSpeed = 5.0f;

    float m_timeSinceAttack;

    protected override void Start() {
        base.Start();
        for(int i = 0; i < m_muzzles.Length; i++) {
            ImpMuzzle muzzle = m_muzzles[i];
            muzzle.NormalizeDir();
        }
        m_timeSinceAttack = 0.0f;
    }

    private void Update() {
        if (m_dead) {
            return;
        }
        m_timeSinceAttack += Time.deltaTime;
        UpdateDirToTarget();
        if(m_target != null && m_timeSinceAttack >= m_attackCoolDown) {
            Attack();
        }
        UpdateAnimParameter();
    }

    private void FixedUpdate() {
        if (m_dead) {
            return;
        }
        if (ShouldUpdateMovement()) {
            UpdateMovement();
        }
    }

    protected override void Hit(Unit target, GameObject source, int damage) {
        Vector3 hitBackDir = target.transform.position - source.transform.position;

        target.OnHit(this.gameObject, source, damage, hitBackDir, m_bulletHitbackSpeed);
    }

    protected override void Die() {
        base.Die();
        m_animator.SetTrigger("Die");
        m_rb2d.gravityScale = 1.0f;
        m_rb2d.mass = 9999f;
        m_dead = true;
    }

    private void Attack() {
        m_animator.SetTrigger("Attack");
        m_timeSinceAttack = 0.0f;
        //play attack anim and generate bullet when anim reaches launch point -> LaunchBullets()
    }

    private void LaunchBullets() {
        foreach(ImpMuzzle muzzle in m_muzzles) {
            EnemyImpBullet bullet = Instantiate(m_bulletPrefab, muzzle.transform.position, muzzle.transform.rotation);

            bullet.InitializeEnemyImpBullet(this, muzzle, m_bulletDamage, m_bulletSpeed, m_bulletDestroyDelay);
            //float angle;
            //angle = Vector2.SignedAngle(new Vector2(-1, 0), muzzle.GetNormalizedDirInWorld());
            //bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
            //Rigidbody2D bulletRb2d = bullet.GetComponent<Rigidbody2D>();
            //bulletRb2d.velocity = muzzle.GetNormalizedDirInWorld() * m_bulletSpeed;
        }
        m_timeSinceAttack = 0.0f;
    }

    private void UpdateMovement() {
        RestrictionArea2D targetArea = m_restrictionAreas[m_targetAreaIndex];
        Vector2 targetAreaCenter = (Vector2)m_target.transform.position + targetArea.centerOffset;
        Vector2 dir = targetAreaCenter - (Vector2)transform.position;

        Vector2 force = dir * m_moveForceRatio;
        force = Vector2.ClampMagnitude(force, m_maxMoveForce);
        m_rb2d.AddForce(force);
        m_rb2d.velocity = Vector2.ClampMagnitude(m_rb2d.velocity, m_speed);
    }

    private bool InsideTargetArea() {
        RestrictionArea2D targetArea = m_restrictionAreas[m_targetAreaIndex];
        Vector2 areaCenter = (Vector2)m_target.transform.position + targetArea.centerOffset;
        if (transform.position.x >= areaCenter.x - targetArea.length / 2 &&
            transform.position.x <= areaCenter.x + targetArea.length / 2 &&
            transform.position.y >= areaCenter.y - targetArea.width / 2 &&
            transform.position.y <= areaCenter.y + targetArea.width / 2) {
            return true;
        }
        return false;
    }
    private bool ShouldUpdateMovement() {
        if(m_target == null || InsideTargetArea()) {
            return false;
        } else {
            return true;
        }
    }

    private void UpdateDirToTarget() {
        if (m_target == null) {
            return;
        }

        if (m_target.transform.position.x < transform.position.x) {
            //m_graphics.localScale = new Vector3(Mathf.Abs(m_graphics.localScale.x), m_graphics.localScale.y, m_graphics.localScale.z);
            transform.localScale = new Vector3(1, 1, 1);
        } else {
            //m_graphics.localScale = new Vector3(-Mathf.Abs(m_graphics.localScale.x), m_graphics.localScale.y, m_graphics.localScale.z);
            transform.localScale = new Vector3(-1, 1, 1);
        }

        foreach(ImpMuzzle muzzle in m_muzzles) {
            Vector2 muzzleToTarget = (m_target.transform.position - muzzle.transform.position).normalized;
            float angle;
            if (transform.localScale.x >= 0) {
                angle = Vector2.SignedAngle(new Vector2(-1, 0), muzzleToTarget);
            } else {
                angle = Vector2.SignedAngle(new Vector2(1, 0), muzzleToTarget);
            }
            muzzle.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void UpdateAnimParameter() {
        m_animator.SetFloat("MoveSpeed", m_rb2d.velocity.magnitude);
    }

    private void OnDrawGizmos() {
        if (m_target != null) {
            for (int i = 0; i < m_restrictionAreas.Length; i++) {
                RestrictionArea2D re2d = m_restrictionAreas[i];
                if (i == m_targetAreaIndex) {
                    Gizmos.color = Color.blue;
                } else {
                    Gizmos.color = Color.red;
                }
                Vector3 center = new Vector3(m_target.transform.position.x + re2d.centerOffset.x, m_target.transform.position.y + re2d.centerOffset.y, 0);
                Gizmos.DrawLine(
                    new Vector3(center.x - re2d.length / 2, center.y + re2d.width / 2, 0f),
                    new Vector3(center.x + re2d.length / 2, center.y + re2d.width / 2, 0f)
                );

                Gizmos.DrawLine(
                    new Vector3(center.x - re2d.length / 2, center.y - re2d.width / 2, 0f),
                    new Vector3(center.x + re2d.length / 2, center.y - re2d.width / 2, 0f)
                );

                Gizmos.DrawLine(
                    new Vector3(center.x - re2d.length / 2, center.y + re2d.width / 2, 0f),
                    new Vector3(center.x - re2d.length / 2, center.y - re2d.width / 2, 0f)
                );

                Gizmos.DrawLine(
                    new Vector3(center.x + re2d.length / 2, center.y + re2d.width / 2, 0f),
                    new Vector3(center.x + re2d.length / 2, center.y - re2d.width / 2, 0f)
                );
            }   //Draw restriction areas
        }

        Gizmos.color = Color.yellow;
        foreach(ImpMuzzle muzzle in m_muzzles) {
            Vector2 dirInWorld = muzzle.GetNormalizedDirInWorld();
            Gizmos.DrawLine(muzzle.transform.position, (Vector3)muzzle.transform.position + (Vector3)dirInWorld);
        }
    }
}
