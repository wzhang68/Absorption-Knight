using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class EnemyProjectile : Unit
{
    private Enemy m_attacker;
    private int m_damage;
    private Transform m_target;
    private float m_bulletSpeed;
    private float m_destoryDelay;

    private bool m_initializeCompleted = false;
    private Rigidbody2D m_rb2d;
    private bool m_hitted = false;
    [SerializeField] private GameObject dmgpopup;

    private void Awake() {
        m_rb2d = GetComponent<Rigidbody2D>();
    }
    
    public override void OnHit(GameObject source, GameObject attacker, int damage, Vector2 hitBackDir, float hitBackSpeed) {
        damage = 0;
        base.OnHit(source, attacker, damage, hitBackDir, hitBackSpeed);
    }

    // Update is called once per frame
    private void Update() {
        if (!m_initializeCompleted) {
            return;
        }

        transform.localScale = new Vector3(m_target.transform.position.x <= transform.position.x ? 1f : -1f, 1f, 1f);

        if (!m_hitted) {
            Vector2 dir = m_target.transform.position - transform.position;
            m_rb2d.velocity = Vector2.ClampMagnitude(dir * m_bulletSpeed, m_bulletSpeed);
        } else {
            Destroy(this.gameObject, m_destoryDelay);
        }
    }

public void InitializeEnemyBullet(Enemy attacker, int damage, Transform target, float speed, float destoryDelay) {
        m_attacker = attacker;
        m_damage = damage;
        m_target = target;
        m_bulletSpeed = speed;
        m_destoryDelay = destoryDelay;
        m_initializeCompleted = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("trigger");
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit != null) {
            if (unit.GetType().Equals(typeof(PlayerUnit))) {
                //-->Enemy.OnAttackZoneHitUnit()-->EnemySorcerer.Hit()
                m_attacker.OnAttackZoneHitUnit(unit, this.gameObject, m_damage);
                Destroy(this.gameObject);
                
                //飞行物的伤害显示
                //人物攻击的伤害显示在 UnitAttackZone.cs line#77-92
                GameObject dmgtext = Instantiate(dmgpopup, unit.transform.position + new Vector3(0f, 1f, 0f),
                    quaternion.identity);
                dmgtext.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(m_damage.ToString());
            } else if (unit.GetType().IsSubclassOf(typeof(Enemy))) {
                if (m_hitted) {
                    m_attacker.OnAttackZoneHitUnit(unit, this.gameObject, (int)(m_damage / 2));
                    Destroy(this.gameObject);
                    
                    //飞行物的伤害显示
                    GameObject dmgtext = Instantiate(dmgpopup, unit.transform.position + new Vector3(0f, 1f, 0f),
                        quaternion.identity);
                    dmgtext.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(m_damage.ToString());
                } else {
                    //this bullet is not hit yet, it will not damage enemy
                    return;
                }
                
            }
        }
    }

    public override void OnAttackZoneHitUnit(Unit hitUnit, GameObject source, int damage) {
        //do nothing
    }

    public override void OnAbsorbZoneHitUnit(Unit hitUnit, GameObject source)
    {
        //do nothing
    }
    public override void OnPrayZoneHitUnit(Unit hitUnit, GameObject source)
    {
        //do nothing
    }

    protected override void OnHitBack(GameObject source, GameObject attacker, Vector2 dir, float hitBackSpeed) {
        dir.Normalize();
        m_rb2d.velocity = dir * 8.0f;
        m_hitted = true;
    }

    protected override void Die() {
        // override and do nothing
    }

}
