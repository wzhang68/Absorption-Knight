using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class UnitAttackZone : MonoBehaviour {

    [SerializeField] bool m_isAnimDrivenAttackZone = false;
    [SerializeField] private GameObject dmgpopup;
    [SerializeField] private GameObject hpsteal_effect;
    [SerializeField] private GameObject heal_effect = null;

    int hitDamage;
    Unit attacker;
    float triggerEnableDelay;
    float destoryDelay;
    string atkType;

    Collider2D collider2d;

    bool initializationCompleted = false;
    bool setupCompleted = false;

    private void Awake() {
        collider2d = GetComponent<Collider2D>();
        collider2d.enabled = false;
    }

    public void InitializeAttackZone(Unit attackerPar, int hitDamagePar, float triggerEnableDelayPar, float destoryDelayPar) {
        attacker = attackerPar;
        hitDamage = hitDamagePar;
        triggerEnableDelay = triggerEnableDelayPar;
        destoryDelay = destoryDelayPar;
        atkType = "attack";
        initializationCompleted = true;
    }
    
    public void InitializeAbsorbZone(Unit attackerPar,  float triggerEnableDelayPar, float destoryDelayPar) {
        attacker = attackerPar;
        triggerEnableDelay = triggerEnableDelayPar;
        destoryDelay = destoryDelayPar;
        atkType = "absorb";
        initializationCompleted = true;
    }
    public void InitializePrayZone(Unit attackerPar,  float triggerEnableDelayPar, float destoryDelayPar) {
        attacker = attackerPar;
        triggerEnableDelay = triggerEnableDelayPar;
        destoryDelay = destoryDelayPar;
        atkType = "pray";
        initializationCompleted = true;
    }
    
    public void InitializeSkillZone(Unit attackerPar, int hitDamagePar, float triggerEnableDelayPar, float destoryDelayPar) {
        attacker = attackerPar;
        hitDamage = hitDamagePar;
        triggerEnableDelay = triggerEnableDelayPar;
        destoryDelay = destoryDelayPar;
        atkType = "skill";
        initializationCompleted = true;
    }

    public void SetHitDamage(int damage) {
        hitDamage = damage;
    }

    private void Update() {
        if (m_isAnimDrivenAttackZone) {
            return;
        }
        if(initializationCompleted && !setupCompleted) {
            Invoke("EnableTrigger", triggerEnableDelay);
            Destroy(this.gameObject, destoryDelay);
            setupCompleted = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if(unit != null) {
            if (atkType == "attack")
            {
                attacker.OnAttackZoneHitUnit(unit, this.gameObject, hitDamage);
                //伤害显示
                //打到怪物会显示伤害
                //打到飞行物会显示音效
                //Damage Text Display: EnemyBullet.cs line#67-70
                if (unit.GetType().Equals(typeof(EnemyBullet)))
                {
                    GameObject dmgtext = Instantiate(dmgpopup, unit.transform.position + new Vector3(0f, 1f, 0f),
                        quaternion.identity);
                    dmgtext.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("PA!");
                }
                // else if (unit.GetType().IsSubclassOf(typeof(Enemy))) 
                else{
                    // If the monster's HP is more than 0, display the damage text. Otherwise, the damage text will be hidden.
                    // EnemyDeath death = collision.gameObject.GetComponent<EnemyDeath>();
                    // string dmgTXT = (!death.isDeath) ? hitDamage.ToString() : "0";
                    GameObject dmgtext = Instantiate(dmgpopup, unit.transform.position + new Vector3(0f, 1f, 0f),
                        quaternion.identity);
                    dmgtext.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(hitDamage.ToString());   
                }
            }else if (atkType == "absorb")
            {
                attacker.OnAbsorbZoneHitUnit(unit,gameObject);
            }
            else if (atkType == "pray")
            {
                attacker.OnPrayZoneHitUnit(unit,gameObject);
            }
            else if (atkType == "skill") 
                //The active skill temporarily be LifeSteal
            {
                
                if (unit.GetType().IsSubclassOf(typeof(Enemy)))
                {
                    attacker.OnAttackZoneHitUnit(unit, this.gameObject, hitDamage);
                    attacker.setAddHP((int)Math.Ceiling(hitDamage*0.3));
                    Instantiate(hpsteal_effect, unit.transform.position, Quaternion.identity, unit.transform);
                    Instantiate(heal_effect, attacker.transform.position, Quaternion.identity,transform);
                    GameObject dmgtext = Instantiate(dmgpopup, unit.transform.position + new Vector3(0f, 1f, 0f),
                        quaternion.identity);
                    dmgtext.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(hitDamage.ToString());  
                    //Heal Stat Display
                    GameObject healtext = Instantiate(dmgpopup, attacker.transform.position + new Vector3(0f, 1f, 0f),
                        quaternion.identity);
                    healtext.transform.GetChild(0).GetComponent<TextMeshPro>().faceColor = new Color32(77, 255, 0, 255);
                    healtext.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(((int)Math.Ceiling(hitDamage*0.3)).ToString());
                }

            }

        }
        if (!m_isAnimDrivenAttackZone) {
            collider2d.enabled = false;
        }
    }

    private void EnableTrigger() {
        collider2d.enabled = true;
    }

}
