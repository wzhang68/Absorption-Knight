using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDeath : MonoBehaviour
{
    private string unitName = string.Empty;
    public bool isDeath = false;
    private Rigidbody2D rb;
    private SpriteRenderer[] m_spriteRenderers;
    private bool stateSent = false;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        unitName = name;
        m_spriteRenderers = transform.GetComponentsInChildren<SpriteRenderer>();
    }
    
    private void DisableScript()
    {
        if (unitName.Contains("EnemySorcerer"))
        {
            EnemySorcerer script = GetComponent<EnemySorcerer>();
            if (script.GetCurrentHP()<=0)
            {
                isDeath = true;
                script.enabled = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                DamageFlash();

            }
            return;
        }
        if (unitName.Contains("EnemySkull"))
        {
            EnemySkull script =GetComponent<EnemySkull>();
            if (script.GetCurrentHP()<=0)
            {
                isDeath = true;
                script.enabled = true;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                DamageFlash();
            }
            return;
        } 
        
        if (unitName.Contains("EnemyCatKnight"))
        {
            EnemyCatKnight script =GetComponent<EnemyCatKnight>();
            if (script.GetCurrentHP()<=0)
            {
                if(!stateSent){
                    string rev = "killonecatknight";
                    if(SceneManager.GetActiveScene().name == "LV-2BossRoom") rev = "level2boss";

                    //send mission manager a message to update mission state
                    MissionManager.getInstance().updateAllMissionStates(rev);
                    Debug.Log(rev);
                    stateSent = true;
                }

                isDeath = true;
                script.enabled = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                DamageFlash();
            }
            return;
        }
        
        if (unitName.Contains("EnemyGoblin"))
        {
            EnemyGoblin script = GetComponent<EnemyGoblin>();
            if (script.GetCurrentHP()<=0)
            {
                if(!stateSent){
                    //send mission manager a message to update mission state
                    MissionManager.getInstance().updateAllMissionStates("killonegoblin");
                    Debug.Log("this goblin is dead");
                    stateSent = true;
                }
                
        
                isDeath = true;
                script.enabled = true;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                DamageFlash();

            }
            return;
        }
        
        
        if (unitName.Contains("EnemyImp"))
        {
            EnemyImp script = GetComponent<EnemyImp>();
            if (script.GetCurrentHP()<=0)
            {
                if(!stateSent){
                    //send mission manager a message to update mission state
                    MissionManager.getInstance().updateAllMissionStates("killoneimp");
                    Debug.Log("this imp is dead");
                    stateSent = true;
                }
                
        
                isDeath = true;
                // script.enabled = false;
                // rb.constraints = RigidbodyConstraints2D.FreezeAll;
                DamageFlash();

            }
            return;
        }

        if (unitName.Contains("EnemyGhoul"))
        {
            EnemyGhoul script = GetComponent<EnemyGhoul>();
            if (script.GetCurrentHP()<=0)
            {
                if(!stateSent){
                    //send mission manager a message to update mission state
                    MissionManager.getInstance().updateAllMissionStates("killoneghoul");
                    Debug.Log("this ghoul is dead");
                    stateSent = true;
                }
                
        
                isDeath = true;
                script.enabled = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                DamageFlash();

            }
            return;
        }

        if (unitName.Contains("EnemyRomancer"))
        {
            EnemyRomancer script = GetComponent<EnemyRomancer>();
            if (script.GetCurrentHP()<=0)
            {
                if(!stateSent){
                    //send mission manager a message to update mission state
                    MissionManager.getInstance().updateAllMissionStates("killoneromancer");
                    Debug.Log("this romancer is dead");
                    stateSent = true;
                }
                
        
                isDeath = true;
                script.enabled = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                DamageFlash();

            }
            return;
        }

        if (unitName.Contains("EnemyWarrior"))
        {
            EnemyWarrior script = GetComponent<EnemyWarrior>();
            if (script.GetCurrentHP()<=0)
            {
                if(!stateSent){
                    //send mission manager a message to update mission state
                    MissionManager.getInstance().updateAllMissionStates("killonewarrior");
                    Debug.Log("this warrior is dead");
                    stateSent = true;
                }
                
        
                isDeath = true;
                script.enabled = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                DamageFlash();

            }
            return;
        }

        if (unitName.Contains("EnemyFireGolem"))
        {
            EnemyFireGolem script = GetComponent<EnemyFireGolem>();
            if (script.GetCurrentHP()<=0)
            {
                if(!stateSent){
                    //send mission manager a message to update mission state
                    MissionManager.getInstance().updateAllMissionStates("killonefiregolem");
                    Debug.Log("this fire golem is dead");
                    stateSent = true;
                }
                
        
                isDeath = true;
                script.enabled = true;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                DamageFlash();

            }
            return;
        }

        if (unitName.Contains("EnemyIceGolem"))
        {
            EnemyIceGolem script = GetComponent<EnemyIceGolem>();
            if (script.GetCurrentHP()<=0)
            {
                if(!stateSent){
                    //send mission manager a message to update mission state
                    MissionManager.getInstance().updateAllMissionStates("killoneicegolem");
                    Debug.Log("this ice golem is dead");
                    stateSent = true;
                }
                
        
                isDeath = true;
                script.enabled = true;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                DamageFlash();

            }
            return;
        }

        if (unitName.Contains("EnemyPhantomKnight"))
        {
            EnemyPhantomKnight script = GetComponent<EnemyPhantomKnight>();
            if (script.GetCurrentHP()<=0)
            {
                if(!stateSent){
                    //send mission manager a message to update mission state
                    MissionManager.getInstance().updateAllMissionStates("killonephantomknight");
                    Debug.Log("this Phantom Knight is dead");
                    stateSent = true;
                }
                
        
                isDeath = true;
                script.enabled = true;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                DamageFlash();

            }
            return;
        }
    }
    
    
    void Update()
    {
        DisableScript();
    }
    
    
    private void DamageFlash() {
        GraphicsTurnToColor(Color.red);
    }
    
    private void GraphicsTurnToColor(Color color) {
        foreach (SpriteRenderer renderer in m_spriteRenderers) {
            renderer.color = color;
        }
    }
    
    
    private IEnumerator Blinkwhite() {
        
        yield return new WaitForSeconds(1f);
        foreach (SpriteRenderer renderer in m_spriteRenderers) {
            renderer.color = Color.white;
        }

    }
}
