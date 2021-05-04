using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using TMPro;

public class PlayerBomb : Unit
{
    // Start is called before the first frame update
    private PlayerUnit m_attacker;
    private int m_damage;
    private float m_destoryDelay;

    private bool m_initializeCompleted = false;
    private Rigidbody2D m_rb2d;
    private bool m_hitted = false;
    [SerializeField] private GameObject dmgpopup;
    [SerializeField] private GameObject bombEffect = null;
    
    private void Awake() {
        m_rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!m_initializeCompleted) {
            return;
        }
    }
    
    public void InitializePlayerBomb(PlayerUnit attacker, int damage, float destoryDelay) {
        m_attacker = attacker;
        m_damage = damage;
        m_destoryDelay = destoryDelay;
        m_initializeCompleted = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit != null) {
          if (unit.GetType().IsSubclassOf(typeof(Enemy))) {
                m_attacker.OnAttackZoneHitUnit(unit, this.gameObject, m_damage);
                Destroy(this.gameObject);
                Instantiate(bombEffect, transform.position, Quaternion.identity);
                //飞行物碰到了怪物
                GameObject dmgtext = Instantiate(dmgpopup, unit.transform.position + new Vector3(0f, 1f, 0f),
                    quaternion.identity);
                dmgtext.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(m_damage.ToString());
            }
        }
    }

    public override void OnHit(GameObject source, GameObject attacker, int damage, Vector2 hitBackDir, float hitBackSpeed) {
        damage = 0;
        base.OnHit(source, attacker, damage, hitBackDir, hitBackSpeed);
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
        //do nothing
    }

    
}
