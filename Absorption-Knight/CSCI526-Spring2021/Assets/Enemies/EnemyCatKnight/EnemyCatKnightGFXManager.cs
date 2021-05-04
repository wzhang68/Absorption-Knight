using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCatKnightGFXManager : MonoBehaviour
{

    [SerializeField] EnemyCatKnight m_enemyCat = null;
    [SerializeField] UnitAttackZone m_tripleAtkZone = null;

    int m_firstAtkDamage;
    int m_secondAtkDamage;
    int m_thirdAtkDamage;

    private void Start() {
        if(m_enemyCat == null || m_tripleAtkZone == null) {
            Debug.LogError("EnemyCatKnight game object is not assigned!");
            this.enabled = false;
        }
        m_enemyCat.SetEnemyCatKnightGFXManager();
    }

    public void InitializeDamage(int first, int second, int third) {
        m_firstAtkDamage = first;
        m_secondAtkDamage = second;
        m_thirdAtkDamage = third;
    }

    private void FirstAtkStart() {
        m_tripleAtkZone.SetHitDamage(m_firstAtkDamage);
        m_enemyCat.UpdateHitBackSpeedForTripleAtk(1);
    }

    private void SecondAtkStart() {
        m_tripleAtkZone.SetHitDamage(m_secondAtkDamage);
        m_enemyCat.UpdateHitBackSpeedForTripleAtk(2);
    }

    private void ThirdAtkStart() {
        m_tripleAtkZone.SetHitDamage(m_thirdAtkDamage);
        m_enemyCat.UpdateHitBackSpeedForTripleAtk(3);
    }
}
