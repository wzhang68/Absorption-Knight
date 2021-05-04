using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


[RequireComponent(typeof(Seeker))]
public class EnemyFireGolem : NormalEnemy{

    [Header("Special Attack Properties")]
    [SerializeField] private UnitAttackZone m_specialAtkZone1;
    [SerializeField] private UnitAttackZone m_specialAtkZone2;
    [SerializeField, Range(0, 1)] private float m_specialAttackProbability = 0.5f;
    [SerializeField] private float m_specialAtk_Delay = 1.0f;
    // [SerializeField] private float m_tripleAtk_Cooldown = 2.0f;
    [Space(5)]
    [SerializeField] private int m_first_atk_damage = 10;
    // [SerializeField] private float m_first_atk_dashSpeed = 1.0f;
    // [SerializeField] private float m_first_atk_hitbackSpeed = 1.5f;
    [Space(5)]
    [SerializeField] private int m_second_atk_damage = 15;
    // [SerializeField] private float m_second_atk_dashSpeed = 1.5f;
    // [SerializeField] private float m_second_atk_hitbackSpeed = 2.0f;
    [Space(5)]
    [SerializeField] float m_minStandOffTime = 1.0f;
    [SerializeField] float m_maxStandOffTime = 1.5f;

    [SerializeField] float m_standOffInnerRange = 2.5f;
    [SerializeField] float m_standOffOutterRange = 3.5f;
    [SerializeField] protected float m_specialAtkCooldown = 3.0f;


    private float m_timeSinceStandOff = 0.0f;
    private float m_timeForThisStandOff = 1f;

    
    protected override void Awake() {
        // initialize enemy properties here
        base.Awake();
        m_endOffset = new Vector3(0f, -2f);
    }

    protected override void Start(){
        base.Start();

        m_unitType = "EnemyFireGolem";
        // m_groundSensor = transform.Find("GroundSensor").GetComponent<EnemyGroundSensor>();
        // m_enemyFireGolemState = EnemyFireGolemState.Idle;
    }

    private void Update() {
        if(m_dead) {
            return;
        }
        m_timeSinceAttack += Time.deltaTime;
        m_timeSinceStandOff += Time.deltaTime;

        CheckGround();
        UpdateHitBackState();
        if (m_normalEnemyState == NormalEnemyState.Idle) {
            IdleAction();
        } else if (m_normalEnemyState == NormalEnemyState.Approaching) {
            ApproachingAction();
        } else if (m_normalEnemyState == NormalEnemyState.Attack) {
            AttackAction();
        } else if(m_normalEnemyState == NormalEnemyState.StandOff){
            StandOffAction();
        }
        UpdateAnimation();
    }

    private void StandOffAction() {
        if (m_timeSinceStandOff >= m_timeForThisStandOff) {
            m_normalEnemyState = NormalEnemyState.Approaching;
            return;
        }
        if (!m_grounded) {
            return;
        }
        float dirx = transform.position.x < m_target.transform.position.x ? 1f : -1f;
        Vector2 dir = new Vector2(dirx, 0);
        m_rb2d.velocity = dir*0.1f;

        // UpdateDir();

        // float distance = Vector2.Distance(transform.position, m_target.transform.position);
        // if (distance <= m_attackRange / 2) {
        //     m_normalEnemyState = NormalEnemyState.Attack;
        //     return;
        // } else if (distance <= m_standOffInnerRange) {
        //     float dirx = transform.position.x < m_target.transform.position.x ? -1f : 1f;
        //     Vector2 dir = new Vector2(dirx, 0);
        //     m_rb2d.velocity = dir * m_speed;
        // } else if (distance >= m_standOffOutterRange) {
        //     float dirx = transform.position.x < m_target.transform.position.x ? 1f : -1f;
        //     Vector2 dir = new Vector2(dirx, 0);
        //     m_rb2d.velocity = dir * m_speed;
        // } else {
        //     //do nothing
        // }

    }

    protected override void AttackAction() {
        if (m_duringAttack) {
            Vector2 dir = new Vector2(transform.position.x >= m_target.transform.position.x ? -1f : 1f, 0f);
            m_rb2d.velocity = dir * 1f;
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, m_target.position);
        if (distanceToTarget > m_attackRange) {
            m_normalEnemyState = NormalEnemyState.Idle;
            return;
        }

        float randomFloat = Random.Range(0f, 1f);
        if(randomFloat <= m_specialAttackProbability){
            if (m_timeSinceAttack > m_attackCooldown) {
                StartCoroutine(DoAttack());
            }
        }else{
            if (m_timeSinceAttack > m_specialAtkCooldown) {
                StartCoroutine(DoSpecialAttack());
            }
        }
       
    }

    private IEnumerator DoSpecialAttack() {
        if (m_specialAtkZone1 == null) {
            Debug.LogError("Special Attack Zone 1 is not assigned!");
            yield break;
        }
        if (m_specialAtkZone2 == null) {
            Debug.LogError("Special Attack Zone 2 is not assigned!");
            yield break;
        }
        m_duringAttack = true;
        m_normalEnemyState = NormalEnemyState.Attack;
        m_timeSinceAttack = 0.0f;
        m_specialAtkZone1.InitializeAttackZone(this, m_first_atk_damage, 0.0f, 0.0f);
        m_specialAtkZone2.InitializeAttackZone(this, m_second_atk_damage, 0.0f, 0.0f);
        yield return new WaitForSeconds(m_specialAtk_Delay);
        if(!m_dead) {
            m_animator.SetTrigger("SpecialAttack");
        }

        // Vector2 dir;
        // dir = new Vector2(transform.position.x >= m_target.transform.position.x ? -1f : 1f, 0f);

        // m_rb2d.velocity = dir * m_first_atk_dashSpeed;
        // yield return new WaitForSeconds(21.0f / 60.0f);     //length of first atk
        // m_rb2d.velocity = dir * m_second_atk_dashSpeed;
        // yield return new WaitForSeconds(21.0f / 60.0f);     //length of second atk
        // m_rb2d.velocity = dir * m_third_atk_dashSpeed;
        // yield return new WaitForSeconds(18.0f / 60.0f);     //length of third atk
        // EnterStandOff();
    }

    private void UpdateSpeed(float atk_dashSpeed){
        Vector2 dir;
        dir = new Vector2(transform.position.x >= m_target.transform.position.x ? -1f : 1f, 0f);
        m_rb2d.velocity = dir * atk_dashSpeed * 0.1f;
        m_hitBackSpeed = atk_dashSpeed * 5;
    }

    protected void EnterStandOff() {
        m_normalEnemyState = NormalEnemyState.StandOff;
        m_timeForThisStandOff = Random.Range(m_minStandOffTime, m_maxStandOffTime);
        m_timeSinceStandOff = 0.0f;

        m_duringAttack = false;

    }
}
