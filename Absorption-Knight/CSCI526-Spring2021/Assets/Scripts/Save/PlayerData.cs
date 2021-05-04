using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
    public string level;
    public int health;
    public int sanity;
    public int maxhealth;
    public int maxsanity;
    public int energyBall;
    public string activeskill;
    public int skillcount;
    public float speed;
    public float jumpforce;
    public Vector3 posLV0 =Vector3.zero;
    public Vector3 posTutorial = Vector3.zero;
    public Vector3 posUI =Vector3.zero;
    public Vector3 posLV1 =Vector3.zero;
    public Vector3 posLV2=Vector3.zero; 
    public Vector3 posLV3=Vector3.zero;
    public Vector3 posLV4=Vector3.zero;
    public Vector3 posLV5=Vector3.zero;
    
    public Vector3 posLV1_Store=Vector3.zero;
    public Vector3 posLV1_BossRoom=Vector3.zero;
    public Vector3 posLV2_Village=Vector3.zero;
    public Vector3 posLV2_Store1=Vector3.zero;
    public Vector3 posLV2_Store2=Vector3.zero;
    public Vector3 posLV2_Store3=Vector3.zero;
    public Vector3 posLV2_Store4=Vector3.zero;
    public Vector3 posLV2_BossRoom=Vector3.zero;
    public Vector3 posLV2_Transition=Vector3.zero;
    public string buffarray = String.Empty;
    public float musicVol;
    public float effectVol;

    // public float[] position =new float[3];

    // Hash Table and Dictionary are not supported by JSONUtility(Cannot convert from them to JSON string), so I need to find other ways.
    // Partially Solved: Imported JSON .NET, USING JSON UTILITY AND JSON.NET AT THE SAME TIME
    // JSON.NET and JSON Utility are shittttttttttttty stuff. MOTHERFUCKER!  STUPID SHIT;
    // NEED TO FIND A MORE CONVENIENT WAY/ A MORE GENERIC WAY TO STORE DATA.
    // I'M KIND LIKE HARD CODING CUZ MIDTERM IS ON WED (3/17????)

    // public Dictionary<string, float[]> posInLevels = new Dictionary<string, float[]>();
    // [JsonProperty("temp")]
    // public List<LevelDetail> temp =new List<LevelDetail>();

    public void Reset()
    {
    level = String.Empty;
    health= 999;
    sanity= 100;
    maxhealth = 999;
    maxsanity = 100;
    energyBall =0;
    musicVol = 0.6f;
    effectVol = 0.6f;
    activeskill = String.Empty;
    skillcount = 0;
    posUI =Vector3.zero;
    posLV0 = Vector3.zero;
    posTutorial = new Vector3(-35.4f,-4.11f,0f);
    posLV1 =new Vector3(-66.51f,-1.49f,0f);
    posLV2=new Vector3(-44.21f,-3.31f,0f); 
    posLV3=new Vector3(-14.3f,35.7f,0f); 
    posLV4=new Vector3(-30.11f,-11.06f,0f); 
    posLV5=new Vector3(-35.69f,-0.44f,0f); 
    posLV1_Store=new Vector3(-18.1f,-5.77f,0f);
    posLV1_BossRoom=new Vector3(-6.67f,2.98f,0f);
    posLV2_Village=new Vector3(-10.99f,-1.89f,0f);
    posLV2_Store1=new Vector3(-6.7f,1.31f,0f);
    posLV2_Store2=new Vector3(6.84f,-3.38f,0f);
    posLV2_Store3=new Vector3(-7.37f,-3.02f,0f);
    posLV2_Store4=new Vector3(-30.11f,-11.06f,0f);
    posLV2_BossRoom=new Vector3(-18.56f,-0.83f,0f);
    posLV2_Transition=new Vector3(6.01f,39.97f,0f);
    }

    public void SetPosInLevels(string level, PlayerUnit player)
    {
        switch (level)
        { 
            case "TempUI":
                posUI = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
                break;
            case "Level-1":
                posLV1 = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
                break;
            case "Level-2":
                posLV2 = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
                break;
            case "Level-3":
                // posLV3 = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
                posLV3 = new Vector3(37.88f, -0.28f, 0f);
                break;
            case "Level-4":
                posLV4 = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
                break;
            case "LV-5":
                posLV5 = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
                break;
            case "LV-1Store":
                posLV1_Store = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
                break;
            case "LV-1BossRoom":
                posLV1_BossRoom = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
                break;
            case "LV-2BossRoom":
                posLV1_BossRoom = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
                break;
            case "Level-2-village":
                posLV2_Village = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
                break;
            case "LV-2Store-1":
                posLV2_Store1 = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
                break;
            case "LV-2Store-2":
                posLV2_Store2 = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
                break;
            case "LV-2Store-3":
                posLV2_Store3 = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
                break;
            case "LV-2Store-4":
                posLV2_Store4 = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
                break;
        }
    }
    
    
    public PlayerData(PlayerUnit player)
    {

        level = SceneManager.GetActiveScene().name;
        // SetPosInLevels(level,player);
        sanity = player.Sanity();
        energyBall = player.EB();
        health = player.HP();
        maxhealth = player.MaxHP();
        maxsanity = player.MaxSan();
        buffarray = player.temp2;
        skillcount = player.GetSkillCount();
        activeskill = player.GetActiveSkillType();
        speed = player.GetTeodoroSpeed();
        jumpforce = player.GetTeodoroJumpForce();
        musicVol = player.musicVol;
        effectVol = player.effectVol;

    }
}

