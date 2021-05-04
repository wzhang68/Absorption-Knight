using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpBullet : Unit {

    private Enemy m_attacker;
    private int m_damage;
    //private Transform m_target;
    //private float m_bulletSpeed;
    private float m_destoryDelay;

    //private bool m_initializeCompleted = false;
    private Rigidbody2D m_rb2d;

    private void Awake() {
        m_rb2d = GetComponent<Rigidbody2D>();
    }

    public override void OnHit(GameObject source, GameObject attacker, int damage, Vector2 hitBackDir, float hitBackSpeed) {
        //do nothing
    }

    public void InitializeEnemyImpBullet(Enemy attacker, ImpMuzzle muzzle, int damage, float speed, float destoryDelay) {
        m_attacker = attacker;
        m_damage = damage;
        m_destoryDelay = destoryDelay;

        float angle;
        angle = Vector2.SignedAngle(new Vector2(-1, 0), muzzle.GetNormalizedDirInWorld());
        gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        m_rb2d.velocity = muzzle.GetNormalizedDirInWorld() * speed;

        Destroy(this.gameObject, m_destoryDelay);
        //m_initializeCompleted = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit != null) {

            if (unit.GetType().Equals(typeof(PlayerUnit))) {
                //-->Enemy.OnAttackZoneHitUnit()-->EnemyImp.Hit()
                m_attacker.OnAttackZoneHitUnit(unit, this.gameObject, m_damage);
                Destroy(this.gameObject);
            } else if (unit.GetType().IsSubclassOf(typeof(Enemy))) {
                return;
            }
        }
    }

    public override void OnAttackZoneHitUnit(Unit hitUnit, GameObject source, int damage) {
        //do nothing
    }

    protected override void OnHitBack(GameObject source, GameObject attacker, Vector2 dir, float hitBackSpeed) {
        //do nothing
    }

    public override void OnAbsorbZoneHitUnit(Unit hitUnit, GameObject source) {
        //do nothing
    }
    
    public override void OnPrayZoneHitUnit(Unit hitUnit, GameObject source) {
        //do nothing
    }
}
