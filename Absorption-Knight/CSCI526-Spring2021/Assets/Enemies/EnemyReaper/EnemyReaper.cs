using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReaper : Enemy {

    [Header("EnemyReaper Properties")]
    [SerializeField] float m_moveForceRatio = 4.0f;
    [SerializeField] float m_maxMoveForce = 4.0f;
    [SerializeField] EnemyReaperMoveArea m_movementArea = null;
    [SerializeField] float m_destUpdateInterval = 5.0f;

    [Header("EnemyReaper Attack Properties")]
    [SerializeField] private ImpMuzzle[] m_muzzles;
    [SerializeField] private float m_attackCoolDown = 1.0f;
    [SerializeField] EnemyReaperBullet m_bulletPrefab = null;
    [SerializeField] int m_bulletDamage = 10;
    [SerializeField] int m_bulletHeal = 20;
    [SerializeField] float m_bulletSpeed = 5.0f;
    [SerializeField] float m_bulletLaunchDelay = 0.1f;
    [SerializeField] float m_bulletDestroyDelay = 6.0f;
    [SerializeField] float m_bulletHitbackSpeed = 5.0f;

    [Header("Blood Charge Properties")]
    [SerializeField] GameObject m_bloodChargePrefab = null;
    [SerializeField] private int m_bloodChargeNeeded = 100;
    [SerializeField] private int m_currentBloodCharge = 0;
    [SerializeField] private float m_bloodChargeInsDelay = 0.5f;
    [SerializeField] private int m_bloodChargeForEachIns = 10;
    [SerializeField, Range(0, 0.2f)] private float m_BloodChargeInsLerpFactor = 0.05f;

    private float m_timeSinceAttack;
    private List<GameObject> m_bloodChargeInsList = new List<GameObject>();
    //private Color m_currentColor;
    private bool m_duringHitDamageFlash;
    private bool m_duringHealFlash;
    private bool m_duringAttack;

#region static variables and functions
    public static List<EnemyReaper> reapersList = new List<EnemyReaper>();
    private static float m_maxChargeDistance = 20f;
    public static void InitializeReapersList() {
        reapersList.AddRange(GameObject.FindObjectsOfType<EnemyReaper>());
        Debug.Log("Reaper list initialized. " + reapersList.Count + " reapers added.");
    }
    public static EnemyReaper GetNearestReaper(Vector3 position) {
        if(reapersList.Count == 0) {
            return null;
        }
        EnemyReaper result = null;
        float currentLowestDis = float.MaxValue;
        foreach(EnemyReaper reaper in reapersList) {
            float distance = Vector3.Distance(position, reaper.transform.position);
            if(distance < currentLowestDis) {
                result = reaper;
                currentLowestDis = distance;
            }
        }
        return result;
        if(currentLowestDis <= m_maxChargeDistance) {
            return result;
        } else {
            return null;
        }
    }
#endregion

    protected override void Start() {
        base.Start();
        InvokeRepeating("SetNextDestionation", 0.1f, m_destUpdateInterval);
        if(reapersList.Count == 0) {
            InitializeReapersList();
        }
        m_timeSinceAttack = 0.0f;
    }

    private void Update() {
        if (m_dead) {
            return;
        }
        if(m_timeSinceAttack >= m_attackCoolDown && m_currentBloodCharge >= m_bloodChargeNeeded) {
            Attack();
        }
        UpdateBloodChargeIns();
        UpdateAnimState();
        m_timeSinceAttack += Time.deltaTime;
    }

    private void FixedUpdate() {
        if (m_dead) {
            return;
        }
        UpdateTransform();
    }

    private void Attack() {
        m_duringAttack = true;
        m_animator.SetTrigger("Attack");
        m_currentBloodCharge -= m_bloodChargeNeeded;

        if (!m_duringHitDamageFlash && !m_duringHealFlash) {
            SetReaperGraphicsColor();
        }
        
        m_timeSinceAttack = 0.0f;
    }

    public void Heal(Unit target, GameObject source, int value) {
        target.OnHeal(source, this.gameObject, value);
    }

    protected override void Die() {
        base.Die();
        m_animator.SetTrigger("Die");
        m_rb2d.gravityScale = -0.1f;
        m_rb2d.mass = 1;
        for(int i = m_bloodChargeInsList.Count - 1; i >= 0; i--) {
            GameObject go = m_bloodChargeInsList[i];
            Destroy(go, 0.01f);
            m_bloodChargeInsList.RemoveAt(i);
        }
        m_bloodChargeInsList.Clear();

        for(int i = EnemyReaper.reapersList.Count -1; i >= 0; i--) {
            EnemyReaper reaper = EnemyReaper.reapersList[i];
            if(reaper.gameObject == this.gameObject) {
                EnemyReaper.reapersList.RemoveAt(i);
            }
        }
        m_dead = true;
    }

    private void DieAnimationFinish() {
        Destroy(gameObject, 0.1f);
    }

    //call in animator
    private void LaunchBullets() {
        StartCoroutine(LaunchBulletsWithDelay());
    }

    IEnumerator LaunchBulletsWithDelay() {
        foreach (ImpMuzzle muzzle in m_muzzles) {
            if (m_dead) {
                yield break;
            }
            EnemyReaperBullet bullet = Instantiate(m_bulletPrefab, muzzle.transform.position, muzzle.transform.rotation);

            bullet.InitializeEnemyImpBullet(this, muzzle, m_bulletDamage, m_bulletHeal, m_bulletSpeed, m_bulletDestroyDelay);
            yield return new WaitForSeconds(m_bulletLaunchDelay);
            m_timeSinceAttack = 0.0f;
            //float angle;
            //angle = Vector2.SignedAngle(new Vector2(-1, 0), muzzle.GetNormalizedDirInWorld());
            //bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
            //Rigidbody2D bulletRb2d = bullet.GetComponent<Rigidbody2D>();
            //bulletRb2d.velocity = muzzle.GetNormalizedDirInWorld() * m_bulletSpeed;
        }
        m_timeSinceAttack = 0.0f;
        m_duringAttack = false;
    }

    protected override void Hit(Unit target, GameObject source, int damage) {
        Vector3 hitBackDir = target.transform.position - source.transform.position;

        target.OnHit(this.gameObject, source, damage, hitBackDir, m_bulletHitbackSpeed);
    }

    public void GenerateBloodChargeFromUnit(Unit unit, int value) {
        StartCoroutine(GenerateBloodChargeBall(unit, value));
        //m_currentBloodCharge += value;
    }

    private IEnumerator GenerateBloodChargeBall(Unit unit, int value) {
        int a = value / 10;
        int b = value % 10;
        if(b != 0) {
            a++;
        }
        for(int i = 0; i < a; i++) {
            if (m_dead) {
                yield break;
            }
            if (unit != null) {
                GameObject ball = Instantiate(m_bloodChargePrefab, unit.transform.position, unit.transform.rotation);
                m_bloodChargeInsList.Add(ball);
                yield return new WaitForSeconds(m_bloodChargeInsDelay);
            }

        }
    }

    private void UpdateBloodChargeIns() {
        for (int i = m_bloodChargeInsList.Count - 1; i >= 0; i--) {
            GameObject go = m_bloodChargeInsList[i];
            if (go == null) {
                continue;
            }
            go.transform.position = Vector3.Lerp(go.transform.position, transform.position, m_BloodChargeInsLerpFactor);
            if (Vector3.Distance(go.transform.position, transform.position) <= 0.8f) {
                //receive blood charge
                m_currentBloodCharge += m_bloodChargeForEachIns;

                if (!m_duringHitDamageFlash && !m_duringHealFlash) {
                    SetReaperGraphicsColor();
                }

                m_bloodChargeInsList.Remove(go);
                Destroy(go, 0.01f);
            }
        }
    }

    protected override void DamageFlash() {
        m_duringHitDamageFlash = true;
        GraphicsTurnToColor(Color.red);
        if (IsInvoking("EndDamageFlash")) {
            CancelInvoke("EndDamageFlash");
        }
        Invoke("EndDamageFlash", 0.2f);
    }

    protected override void HealFlash() {
        m_duringHealFlash = true;
        GraphicsTurnToColor(Color.green);
        if (IsInvoking("EndDamageFlash")) {
            CancelInvoke("EndDamageFlash");
        }
        Invoke("EndDamageFlash", 0.2f);
    }

    private void EndDamageFlash() {
        SetReaperGraphicsColor();
        m_duringHitDamageFlash = false;
        m_duringHealFlash = false;
    }

    private void SetReaperGraphicsColor() {
        Color newColor = Color.Lerp(Color.white, new Color(255f, 0f, 228f), (float)m_currentBloodCharge / (float)m_bloodChargeNeeded); ;
        foreach (SpriteRenderer renderer in m_spriteRenderers) {
            renderer.color = newColor;
            //m_currentColor = newColor;
        }
    }

    protected override void InitializeActivateZones() {
        //do nothing
    }

    private void SetNextDestionation() {
        m_destination = m_movementArea.GetRandomTransform();
        UpdateEnemyReaperPath();
    }

    private void UpdateTransform() {
        if (m_destination == null || m_path == null || m_currentWayPointIndex > m_path.vectorPath.Count) {
            return;
        }
        if(m_currentWayPointIndex == m_path.vectorPath.Count) {
            SetNextDestionation();
            return;
        }

        Debug.DrawLine(m_path.vectorPath[m_currentWayPointIndex], m_path.vectorPath[m_currentWayPointIndex] + new Vector3(0, 1, 0), Color.red);

        Vector2 dir = ((Vector2)m_path.vectorPath[m_currentWayPointIndex] - m_rb2d.position).normalized;
        
        Vector2 force = dir * m_moveForceRatio;
        force = Vector2.ClampMagnitude(force, m_maxMoveForce);
        m_rb2d.AddForce(force);
        m_rb2d.velocity = Vector2.ClampMagnitude(m_rb2d.velocity, m_speed);
        //Debug.Log(m_rb2d.velocity);

        float distance = Vector2.Distance(m_rb2d.position, m_path.vectorPath[m_currentWayPointIndex]);
        if (distance < nextWaypointDistance) {
            m_currentWayPointIndex++;
        }

        if (m_duringAttack) {
            return;
        }

        int flipIndex = 1;
        if (force.x >= 0.01f) {
            //m_graphics.localScale = new Vector3(3f, 3f, 3f);
            flipIndex = -1;
        } else if (force.x <= -0.01f) {
            //m_graphics.localScale = new Vector3(-3f, 3f, 3f);
            flipIndex = 1;
        }
        Vector3 scale = m_graphics.localScale;
        m_graphics.localScale = new Vector3(flipIndex * Mathf.Abs(scale.x), scale.y, scale.z);
    }

    private void UpdateEnemyReaperPath() {
        UpdatePath("BirdNavMesh");
    }

    private void UpdateAnimState() {
        m_animator.SetFloat("Speed", m_rb2d.velocity.magnitude);
    }

    public bool GetDead() {
        return m_dead;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        foreach (ImpMuzzle muzzle in m_muzzles) {
            Vector2 dirInWorld = muzzle.GetNormalizedDirInWorld();
            Gizmos.DrawLine(muzzle.transform.position, (Vector3)muzzle.transform.position + (Vector3)dirInWorld);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, nextWaypointDistance);

        //Gizmos.color = Color.cyan;
        //Gizmos.DrawWireSphere(transform.position, m_maxChargeDistance);
    }
}
