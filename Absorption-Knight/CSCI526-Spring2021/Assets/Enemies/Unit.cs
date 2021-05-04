using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour {
    
    [Header("Unit Properties")]
    [SerializeField] protected int m_currentHealth = 100;
    [SerializeField] protected int m_maxHealth = 100;
    [SerializeField] protected Transform m_graphics = null;
    
    [SerializeField] protected int m_currentSanity = 100;
    [SerializeField] protected int m_maxSanity = 100;
    [SerializeField] protected int m_sanDropAfterKill = 0;
    //Energy Ball Related
    [SerializeField] protected int m_energyBallAfterKill = 0;
    [SerializeField] protected int m_currentEB = 0;
    private int m_maxEB = 999999999;
    [SerializeField] protected string m_activeSkillType =String.Empty;
    [SerializeField] protected string m_skillType =String.Empty;  // Like skill1, fireball .......
    [SerializeField] protected string m_unitType = String.Empty;  // Like player, normal, sorcerer.....
    

    protected SpriteRenderer[] m_spriteRenderers;
    

    protected virtual void Start() {
        m_spriteRenderers = m_graphics.GetComponents<SpriteRenderer>();
        // m_currentHealth = m_maxHealth;
    }

    public virtual void OnHit(GameObject source, GameObject attacker, int damage, Vector2 hitBackDir, float hitBackSpeed) {
        DamageFlash();
        OnHitBack(source, attacker, hitBackDir, hitBackSpeed);
        AddHp(-damage);
        // AbsorbState();
    }

    public virtual void OnHeal(GameObject source, GameObject healer, int value) {
        HealFlash();
        AddHp(value);
    }

    public abstract void OnAttackZoneHitUnit(Unit hitUnit, GameObject source, int damage);

    public abstract void OnAbsorbZoneHitUnit(Unit hitUnit, GameObject source);
    
    public abstract void OnPrayZoneHitUnit(Unit hitUnit, GameObject source);

    protected virtual void DamageFlash() {
        GraphicsTurnToColor(Color.red);
        Invoke("GraphicsTurnToWhite", 0.2f);
    }

    protected virtual void HealFlash() {
        GraphicsTurnToColor(Color.green);
        Invoke("GraphicsTurnToWhite", 0.2f);
    }
    
    protected abstract void OnHitBack(GameObject source, GameObject attacker, Vector2 dir, float hitBackSpeed);

    /// <summary>
    /// Add value to health. Positive to add. Negative to sub
    /// If the unit type is "player", destory Teodoro. If not, the monster will be absorbable.
    /// </summary>
    /// <param name="value">Positive to add. Negative to sub</param>
    protected void AddHp(int value) {
        if(value < 0) {
            EnemyReaper reaper = EnemyReaper.GetNearestReaper(this.transform.position);
            if(reaper != null && !reaper.GetDead()) {
                reaper.GenerateBloodChargeFromUnit(this, -value);
            }
        }
        m_currentHealth = Mathf.Clamp(m_currentHealth + value, 0, m_maxHealth);
        if (m_currentHealth <= 0) {
            Die();
        }
    }


    //BUFF Related
    protected void AddMaxHp(int value)
    {
        m_maxHealth = m_maxHealth + value;
    }
    
    protected void AddMaxSan(int value)
    {
        m_maxSanity = m_maxSanity + value;
    }
    

    /// <summary>
    /// Add value to energy ball. Positive to add. Negative to sub
    /// </summary>
    /// <param name="value">Positive to add. Negative to sub</param>
    protected void AddEB(int value)
    {
        if (m_currentSanity < 70)
        {
            m_currentEB = Mathf.Clamp(m_currentEB + (int)Math.Ceiling(value*0.8), 0, m_maxEB);
        }
        else
        {
            m_currentEB = Mathf.Clamp(m_currentEB + value, 0, m_maxEB);
        }

    }

    /// <summary>
    /// Add value to sanity. Positive to add. Negative to sub
    /// </summary>
    /// <param name="value">Positive to add. Negative to sub</param>
    protected void AddSan(int value)
    {
        m_currentSanity = Mathf.Clamp(m_currentSanity + value, 0, m_maxSanity);
        if (value < 0)
        {
            SoundManager.PlaySound("sanityDrop");
        }
        else if(value >0)
        {
            SoundManager.PlaySound("sanityUp");
        }
    }
    
    protected virtual void AbsorbState() {
       //DO NOTHING 
    }
    
    protected virtual void Die() {
        //Destroy(this.gameObject);
    }

    protected void GraphicsTurnToColor(Color color) {
        foreach (SpriteRenderer renderer in m_spriteRenderers) {
            renderer.color = color;
        }
    }

    protected void GraphicsTurnToWhite() {
        foreach (SpriteRenderer renderer in m_spriteRenderers) {
            renderer.color = Color.white;
        }
    }
    
    
    /// <summary>
    /// GET METHODS 
    /// </summary>
    /// <returns></returns>
    public int GetEBAfterKill()
    {
        return m_energyBallAfterKill;
    }
    
    public int GetSanAfterKill()
    {
        return m_sanDropAfterKill;
    }
    public int GetCurrentHP()
    {
        return m_currentHealth;
    }

    /// <summary>
    /// Get Skill name from units
    /// </summary>
    /// <returns></returns>
    public string GetActiveSkill()
    {
        return m_activeSkillType;
    }
    
    /// <summary>
    /// Get BUFF name from units
    /// </summary>
    /// <returns></returns>
    public string GetSkill()
    {
        return m_skillType;
    }
    public int getMaxHealth(){
        return m_maxHealth;
    }

    public int getCurHealth(){
        return m_currentHealth;
    }
    
    public void setAddHP(int val)
    {
        AddHp(val);
    }
}
