using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum EnemyCatKnightState {
    Idle,
    Approaching,
    Attack,
    StandOff
}

[RequireComponent(typeof(Seeker))]
public class EnemyCatKnight : Enemy {

    [Header("Enemy Cat Knight Properties")]
    //[SerializeField] float jumpForce = 6.0f;
    [SerializeField] float m_attackRange = 2.0f;
    [SerializeField] float m_standOffInnerRange = 2.5f;
    [SerializeField] float m_standOffOutterRange = 3.5f;
    [SerializeField] float m_minStandOffTime = 2.0f;
    [SerializeField] float m_maxStandOffTime = 5.0f;
    [SerializeField] private EnemyCatKnightState m_enemyCatKnightState = EnemyCatKnightState.Idle;
    [SerializeField] private bool m_grounded = false;
    [SerializeField] private bool m_duringHitBack = false;
    [SerializeField] private bool m_doingTripleAtk = false;

    [Header("Enemy Cat Knight Attack1 Properties")]
    [SerializeField] private UnitAttackZone m_attackZone1 = null;
    [SerializeField] private float m_atk_1_hitBackSpeed = 6f;
    [SerializeField] private float m_atk_1_selfHitBackSpeed = 5f;
    [SerializeField] private int m_attack_1_Damage = 0;
    [SerializeField] private float m_atk_1_Delay = 0.5f;
    [SerializeField] private float m_atk_1_Cooldown = 1.5f;
    [SerializeField] private float m_atk_1_TriggerEnableDelay = 0.1f;
    [SerializeField] private float m_atk_1_DestoryDelay = 1f;

    [Header("Triple Attack Properties")]
    [SerializeField] private UnitAttackZone m_tripleAtkZone;
    [SerializeField] private EnemyCatKnightGFXManager m_catGFXManager;
    [SerializeField] private int m_tripleAttackhpthreshold = 50;
    [SerializeField, Range(0, 1)] private float m_tripleAttackProbability = 0.5f;
    [SerializeField] private float m_tripleAtk_Delay = 1.0f;
    [SerializeField] private float m_tripleAtk_Cooldown = 2.0f;
    [Space(5)]
    [SerializeField] private int m_first_atk_damage = 5;
    [SerializeField] private float m_first_atk_dashSpeed = 1.0f;
    [SerializeField] private float m_first_atk_hitbackSpeed = 1.5f;
    [Space(5)]
    [SerializeField] private int m_second_atk_damage = 10;
    [SerializeField] private float m_second_atk_dashSpeed = 1.5f;
    [SerializeField] private float m_second_atk_hitbackSpeed = 2.0f;
    [Space(5)]
    [SerializeField] private int m_third_atk_damage = 15;
    [SerializeField] private float m_third_atk_dashSpeed = 2.0f;
    [SerializeField] private float m_third_atk_hitbackSpeed = 2.5f;

    private Sensor_CatKnight m_groundSensor;
    private Sensor_CatKnight_Wall m_leftWallSensor;
    private Sensor_CatKnight_Wall m_rightWallSensor;

    private float m_timeSinceAttack = 0.0f;
    private float m_timeSinceHitBack = 0.0f;
    private float m_timeSinceStandOff = 0.0f;
    private float m_timeForThisStandOff = 2.5f;

    private float m_currentAtkHitbackSpeed;

    protected override void Awake() {
        base.Awake();

        m_animator = m_graphics.GetComponent<Animator>();

        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_CatKnight>();
        m_leftWallSensor = transform.Find("PlayerLeftWallSensor").GetComponent<Sensor_CatKnight_Wall>();
        m_rightWallSensor = transform.Find("PlayerRightWallSensor").GetComponent<Sensor_CatKnight_Wall>();
    }

    protected override void Start() {
        base.Start();
        m_enemyCatKnightState = EnemyCatKnightState.Idle;
        m_timeSinceAttack = 100.0f;     //big number to avoid first cool down
        //InvokeRepeating("UpdateCatKnightPath", 0f, 1f);
    }

    private void Update() {
        m_timeSinceAttack += Time.deltaTime;
        m_timeSinceStandOff += Time.deltaTime;

        CheckGround();
        UpdateHitBackState();
        if (m_enemyCatKnightState == EnemyCatKnightState.Idle) {
            IdleAction();
        } else if (m_enemyCatKnightState == EnemyCatKnightState.Approaching) {
            ApproachingAction();
        } else if (m_enemyCatKnightState == EnemyCatKnightState.Attack) {
            AttackAction();
        } else if (m_enemyCatKnightState == EnemyCatKnightState.StandOff) {
            StandOffAction();
        }
        UpdateAnimation();
    }

    private void IdleAction() {
        m_animator.SetInteger("AnimState", 0);  //set to idle
        if (m_destination != null) {
            m_enemyCatKnightState = EnemyCatKnightState.Approaching;
            if (!IsInvoking("UpdateCatKnightPath")) {
                InvokeRepeating("UpdateCatKnightPath", 0f, 0.5f);
            }
        }
    }

    private void ApproachingAction() {
        if (m_destination == null || m_path == null || m_currentWayPointIndex >= m_path.vectorPath.Count) {
            return;
        }
        if (!m_grounded) {
            return;
        }

        UpdateDirToTarget();

        Debug.DrawLine(m_path.vectorPath[m_currentWayPointIndex], m_path.vectorPath[m_currentWayPointIndex] + new Vector3(0, 1, 0), Color.red);
        Vector2 dir = ((Vector2)m_path.vectorPath[m_currentWayPointIndex] - m_rb2d.position).normalized;
        float distanceToNextWaypoint = Vector2.Distance(m_rb2d.position, m_path.vectorPath[m_currentWayPointIndex]);
        if (distanceToNextWaypoint < nextWaypointDistance) {
            m_currentWayPointIndex++;
        } else {
            if (!m_duringHitBack) {
                if (dir.x >= 0.25f && m_rightWallSensor.State()) {
                    m_rb2d.velocity = new Vector2(m_speed, m_rb2d.velocity.y);
                } else if (dir.x <= -0.25f && m_leftWallSensor.State()) {
                    m_rb2d.velocity = new Vector2(-m_speed, m_rb2d.velocity.y);
                }
            }
        }

        float distanceToTarget = Vector2.Distance(transform.position, m_target.position);
        if (distanceToTarget <= m_attackRange) {
            m_enemyCatKnightState = EnemyCatKnightState.Attack;
        }
    }

    private void AttackAction() {
        if (m_doingTripleAtk) {
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, m_target.position);
        if (distanceToTarget > m_attackRange) {
            m_enemyCatKnightState = EnemyCatKnightState.Idle;
            return;
        }

        float randomFloat = Random.Range(0f, 1f);
        if(m_currentHealth <= m_tripleAttackhpthreshold && randomFloat <= m_tripleAttackProbability) {
            if(m_timeSinceAttack > m_tripleAtk_Cooldown) {
                StartCoroutine(DoTripleAttack());
            }
        } else {
            if (m_timeSinceAttack > m_atk_1_Cooldown) {
                StartCoroutine(DoAttackOne());
            }
        }


        //if (m_timeSinceAttack > m_atk_1_Cooldown) {
        //if (m_timeSinceAttack > m_tripleAtk_Cooldown) {
        //    float distanceToTarget = Vector2.Distance(transform.position, m_target.position);
        //    if (distanceToTarget > m_attackRange) {
        //        m_enemyCatKnightState = EnemyCatKnightState.Idle;
        //        return;
        //    }
        //    //StartCoroutine(DoAttackOne());
        //    StartCoroutine(DoTripleAttack());
        //}
    }

    private IEnumerator DoAttackOne() {
        m_currentAtkHitbackSpeed = m_atk_1_hitBackSpeed;
        m_enemyCatKnightState = EnemyCatKnightState.Attack;
        m_timeSinceAttack = 0.0f;
        yield return new WaitForSeconds(m_atk_1_Delay);
        float attackMoveSpeed = transform.localScale.x >= 0 ? 1.0f : -1.0f;
        if (!m_duringHitBack) {
            m_rb2d.velocity = new Vector2(attackMoveSpeed * 1.2f, m_rb2d.velocity.y);
        }
        m_animator.SetTrigger("Attack1");
        UnitAttackZone attackZone = Instantiate(m_attackZone1, transform, false);
        attackZone.InitializeAttackZone(this, m_attack_1_Damage, m_atk_1_TriggerEnableDelay, m_atk_1_DestoryDelay);
    }

    private IEnumerator DoTripleAttack() {
        if (m_tripleAtkZone == null) {
            Debug.LogError("Triple Attack Zone is not assigned!");
            yield break;
        }
        m_doingTripleAtk = true;
        m_enemyCatKnightState = EnemyCatKnightState.Attack;
        m_timeSinceAttack = 0.0f;
        m_tripleAtkZone.InitializeAttackZone(this, m_first_atk_damage, 0.0f, 0.0f);
        yield return new WaitForSeconds(m_tripleAtk_Delay);
        m_animator.SetTrigger("Attack4");

        Vector2 dir;
        dir = new Vector2(transform.position.x >= m_target.transform.position.x ? -1f : 1f, 0f);

        m_rb2d.velocity = dir * m_first_atk_dashSpeed;
        yield return new WaitForSeconds(21.0f / 60.0f);     //length of first atk
        m_rb2d.velocity = dir * m_second_atk_dashSpeed;
        yield return new WaitForSeconds(21.0f / 60.0f);     //length of second atk
        m_rb2d.velocity = dir * m_third_atk_dashSpeed;
        yield return new WaitForSeconds(18.0f / 60.0f);     //length of third atk
        EnterStandOff();
        m_doingTripleAtk = false;
    }

    private void StandOffAction() {
        if (m_timeSinceStandOff >= m_timeForThisStandOff) {
            m_enemyCatKnightState = EnemyCatKnightState.Approaching;
            return;
        }
        if (!m_grounded) {
            return;
        }
        UpdateDirToTarget();

        float distance = Vector2.Distance(transform.position, m_target.transform.position);
        if (distance <= m_attackRange / 2) {
            m_enemyCatKnightState = EnemyCatKnightState.Attack;
            return;
        } else if (distance <= m_standOffInnerRange) {
            float dirx = transform.position.x < m_target.transform.position.x ? -1f : 1f;
            Vector2 dir = new Vector2(dirx, 0);
            m_rb2d.velocity = dir * m_speed;
        } else if (distance >= m_standOffOutterRange) {
            float dirx = transform.position.x < m_target.transform.position.x ? 1f : -1f;
            Vector2 dir = new Vector2(dirx, 0);
            m_rb2d.velocity = dir * m_speed;
        } else {
            //do nothing
        }

    }

    public override void OnAttackZoneHitUnit(Unit hitUnit, GameObject source, int damage) {
        base.OnAttackZoneHitUnit(hitUnit, source, damage);      //Hit(hitUnit, source, damage);
        if (!m_doingTripleAtk) {
            Invoke("EnterStandOff", 0.5f);
        }
    }

    private void EnterStandOff() {
        m_enemyCatKnightState = EnemyCatKnightState.StandOff;
        m_timeForThisStandOff = Random.Range(m_minStandOffTime, m_maxStandOffTime);
        m_timeSinceStandOff = 0.0f;
    }

    protected override void Hit(Unit hitUnit, GameObject source, int damage) {
        Vector2 hitBackDir;
        if (transform.position.x >= hitUnit.transform.position.x) {
            hitBackDir = new Vector2(-1f, 0f);
        } else {
            hitBackDir = new Vector2(1f, 0f);
        }
        if (!m_doingTripleAtk) {
            //self hit back
            m_rb2d.velocity = -hitBackDir * m_atk_1_selfHitBackSpeed;
        }

        //Debug.Log("Hitback speed: " + m_currentAtkHitbackSpeed);
        hitUnit.OnHit(this.gameObject, source, damage, hitBackDir, m_currentAtkHitbackSpeed);
    }

    protected override void OnHitBack(GameObject source, GameObject attacker, Vector2 hitBackDir, float hitBackSpeed) {
        base.OnHitBack(source, attacker, hitBackDir, hitBackSpeed);
        m_duringHitBack = true;
        m_timeSinceHitBack = 0.0f;
    }
    private void UpdateAnimation() {
        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_rb2d.velocity.y);

        if (Mathf.Abs(m_rb2d.velocity.x) > Mathf.Epsilon) {
            m_animator.SetInteger("AnimState", 1);  //set to run
        } else {
            m_animator.SetInteger("AnimState", 0);  //set to idle
        }
    }

    private void UpdateCatKnightPath() {
        if (m_grounded && m_enemyCatKnightState == EnemyCatKnightState.Approaching) {
            UpdatePath("CatKnightNavMesh");
        }
    }

    private void CheckGround() {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State()) {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
            m_path = null;
            UpdateCatKnightPath();

            //m_doubleJumpMarker = true;
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State()) {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }
    }

    private void UpdateHitBackState() {
        if (m_duringHitBack) {
            m_timeSinceHitBack += Time.deltaTime;
            if (m_timeSinceHitBack > 2.0f || Mathf.Abs(m_rb2d.velocity.x) <= 0.01f) {
                m_duringHitBack = false;
            }
        }
    }

    private void UpdateDirToTarget() {
        if (m_target != null) {
            if (m_target.position.x >= transform.position.x) {
                //m_graphics.localScale = new Vector3(1f, 1f, 1f);
                transform.localScale = new Vector3(1f, 1f, 1f);
                m_rightWallSensor.transform.localScale = new Vector3(1f, 1f, 1f);
                m_leftWallSensor.transform.localScale = new Vector3(1f, 1f, 1f);
            } else {
                //m_graphics.localScale = new Vector3(-1f, 1f, 1f);
                transform.localScale = new Vector3(-1f, 1f, 1f);
                m_rightWallSensor.transform.localScale = new Vector3(-1f, 1f, 1f);
                m_leftWallSensor.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
    }

    public void SetEnemyCatKnightGFXManager() {
        if (m_catGFXManager == null) {
            Debug.LogError("GFXManager is not assigned");
        } else {
            m_catGFXManager.InitializeDamage(m_first_atk_damage, m_second_atk_damage, m_third_atk_damage);
        }
    }

    public void UpdateHitBackSpeedForTripleAtk(int index) {
        if (index == 1) {
            m_currentAtkHitbackSpeed = m_first_atk_hitbackSpeed;
        } else if (index == 2) {
            m_currentAtkHitbackSpeed = m_second_atk_hitbackSpeed;
        }else if (index == 3) {
            m_currentAtkHitbackSpeed = m_third_atk_hitbackSpeed;
        } else {
            Debug.LogError("Invalid Index");
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, nextWaypointDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_standOffInnerRange);
        Gizmos.DrawWireSphere(transform.position, m_standOffOutterRange);
    }
}
