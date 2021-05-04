using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public abstract class Enemy : Unit {

    [Header("Enemy properties")]
    [SerializeField] protected float m_speed = 4.0f;
    [SerializeField] protected Transform m_target = null;
    [SerializeField] protected float nextWaypointDistance = 0.5f;
    [SerializeField] protected EnemyActivateZone[] m_activateZones = null;
    [SerializeField] public string movePath;
    [SerializeField] public bool dontDisplayHPGauge;
    [SerializeField] public float HPGaugeOffset;

    protected bool m_dead = false;

    protected Transform m_destination;
    protected Animator m_animator;
    protected Rigidbody2D m_rb2d;

    protected Path m_path;
    protected int m_currentWayPointIndex = 0;
    protected Seeker m_seeker;

    protected virtual void Awake() {
        m_animator = GetComponent<Animator>();
        m_rb2d = GetComponent<Rigidbody2D>();
        m_seeker = GetComponent<Seeker>();
    }

    protected override void Start() {
        base.Start();
        InitializeActivateZones();

        //if this option is on, create an hp gauge object under it
        if(!dontDisplayHPGauge && this.gameObject.GetComponentInChildren<MonsterHPGauge>() == false){
            GameObject temp = (GameObject)Instantiate(GameManager.instance.HPGauge);
            temp.transform.SetParent(this.transform);
            if(HPGaugeOffset != 0f) temp.gameObject.GetComponent<MonsterHPGauge>().HPGaugeOffset.y += HPGaugeOffset;
        }
    }

    protected virtual void InitializeActivateZones() {
        if (m_activateZones.Length == 0) {
            Debug.LogError("Activate zones of " + gameObject.name + " may not be assigned");
        }
        foreach (EnemyActivateZone zone in m_activateZones) {
            zone.SetLinkedEnemy(this);
        }
    }
    

    public override void OnAttackZoneHitUnit(Unit hitUnit, GameObject source, int damage) {
        Hit(hitUnit, source, damage);
        
    }

    public override void OnHeal(GameObject source, GameObject healer, int value) {
        if (m_dead) {
            return;
        } else {
            base.OnHeal(source, healer, value);
        }

    }

    protected override void Die() {
        m_dead = true;
        base.Die();
    }

    public override void OnAbsorbZoneHitUnit(Unit hitUnit, GameObject source) {
        // Do Nothing
    }

    public override void OnPrayZoneHitUnit(Unit hitUnit, GameObject source)
    {
        // Do Nothing
    }

    protected abstract void Hit(Unit target, GameObject source, int damage);

    public override void OnHit(GameObject source, GameObject attacker, int damage, Vector2 hitBackDir, float hitBackSpeed) {
        if (m_dead) {
            return;
        } else {
            base.OnHit(source, attacker, damage, hitBackDir, hitBackSpeed);
        }
    }

    protected override void OnHitBack(GameObject source, GameObject attacker, Vector2 dir, float hitBackSpeed) {
        if (m_dead) {
            return;
        }
        //Vector3 dir = transform.position - source.transform.position;
        dir.Normalize();
        m_rb2d.velocity = dir * hitBackSpeed;
    }

    public virtual void OnPlayerEnterActivateZone(PlayerUnit player, EnemyActivateZone zone) {
        m_target = player.gameObject.transform;
        m_destination = m_target;
        Destroy(zone.gameObject.transform.parent.gameObject);
    }

    protected void UpdatePath(string navMeshGridName) {
        if(m_destination == null) {
            //Debug.Log(gameObject.name + " does not have a destination but try to update path");
            return;
        }
        if (m_seeker.IsDone()) {
            string temp = navMeshGridName;
            if(movePath != "") temp = movePath;
            m_seeker.StartPath(m_rb2d.position, m_destination.position, OnPathComplete, GraphMask.FromGraphName(temp));
        }
    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            m_path = p;
            m_currentWayPointIndex = 1;
        }
    }

    public void SetTarget(Transform target) {
        m_target = target;
        //m_destination = target;
    }

    // protected override void AbsorbState()
    // {
    //     if (m_currentHealth <= 0)
    //     {
    //         m_target = null;
    //         m_speed = 0f;
    //         m_rb2d.constraints = RigidbodyConstraints2D.FreezePosition;
    //
    //     }
    // }
}
