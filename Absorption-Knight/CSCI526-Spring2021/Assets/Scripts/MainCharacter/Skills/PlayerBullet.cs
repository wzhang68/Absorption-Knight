using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;


public class PlayerBullet : Unit
{

    private PlayerUnit m_attacker;
    private int m_damage;
    // private Transform m_target;
    private float m_bulletSpeed;
    private float m_destoryDelay;

    private bool m_initializeCompleted = false;
    private Rigidbody2D m_rb2d;
    private bool m_hitted = false;
    [SerializeField] private GameObject dmgpopup;
    [SerializeField] private GameObject bulletEffect = null;
    
    
    private void Awake() {
        m_rb2d = GetComponent<Rigidbody2D>();
    }
    
    public override void OnHit(GameObject source, GameObject attacker, int damage, Vector2 hitBackDir, float hitBackSpeed) {
        damage = 0;
        base.OnHit(source, attacker, damage, hitBackDir, hitBackSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_initializeCompleted) {
            return;
        }
        if (!m_hitted) {
            Vector2 dir = new Vector2(transform.position.x,0f) - new Vector2(m_attacker.transform.position.x,0f);
            transform.localScale = new Vector3(dir.x <0 ? -1f : 1f, 1f, 1f);
            m_rb2d.velocity = Vector2.ClampMagnitude(dir * m_bulletSpeed, m_bulletSpeed);
        } else {
            Destroy(this.gameObject, m_destoryDelay);
        }

    }

    
    //在EnemyBullet的基础上做了调整 
    //如果碰到了是个敌人， 造成伤害 生成攻击效果然后消失
    //如果不是敌人，但他是个Unit 造成0伤害 有攻击效果 然后消失。 就像是反弹蘑菇法师的魔法球
    //如果碰撞体什么也不是 就直接消失
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit != null) {
            if (unit.GetType().Equals(typeof(EnemyBullet)))

            {
                m_attacker.OnAttackZoneHitUnit(unit, this.gameObject, 0);
                Destroy(this.gameObject);
                Instantiate(bulletEffect, unit.transform.position, Quaternion.identity);
                //飞行物碰到了飞行物的伤害显示
                GameObject dmgtext = Instantiate(dmgpopup, unit.transform.position + new Vector3(0f, 1f, 0f),
                    quaternion.identity);
                dmgtext.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("PA!");
            }
            else if (unit.GetType().IsSubclassOf(typeof(Enemy))) {
                m_attacker.OnAttackZoneHitUnit(unit, this.gameObject, m_damage);
                Destroy(this.gameObject);
                Instantiate(bulletEffect, unit.transform.position, Quaternion.identity);
                //飞行物碰到了怪物
                GameObject dmgtext = Instantiate(dmgpopup, unit.transform.position + new Vector3(0f, 1f, 0f),
                    quaternion.identity);
                dmgtext.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(m_damage.ToString());
            }

        }

    }
    
    public void InitializePlayerBullet(PlayerUnit attacker, int damage, float speed, float destoryDelay) {
        m_attacker = attacker;
        m_damage = damage;
        m_bulletSpeed = speed;
        m_destoryDelay = destoryDelay;
        m_initializeCompleted = true;
    }
    
    public override void OnAttackZoneHitUnit(Unit hitUnit, GameObject source, int damage)
    {
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

    protected override void OnHitBack(GameObject source, GameObject attacker, Vector2 dir, float hitBackSpeed)
    {
        // dir.Normalize();
        // m_rb2d.velocity = dir * 8.0f;
        // m_hitted = true;
    }
}
