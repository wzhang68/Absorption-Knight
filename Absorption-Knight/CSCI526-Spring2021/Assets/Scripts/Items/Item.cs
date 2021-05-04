using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item :MonoBehaviour
{
    public enum ItemType
    {
        HpPortion, SanPortion, Weapon, Armor, Special, EnergyBall
    }
    [Header("General Information")]
    public ItemType ItemsType = new ItemType();
    public string name;
    public int cost;
    
    [Header("HpPortion Properties")]
    public int hpchange;

    [Header("SanPortion Properties")]
    public int sanchange;
    
    [Header("Weapon Properties")]
    public int ATKchange;
    
    [Header("Amor Properties")]
    public int DEFchange;
    
    [Header("Special Item Properties")]
    public int NumOfSkillchange;
    
    [Header("EnergyBall Properties")]
    public int EBReward;

    [HideInInspector]
    public bool isPurchased = false;


    private void Start()
    {
        if (transform.childCount>0)
        {
            transform.GetChild(0).GetComponent<TextMesh>().text = cost.ToString();   
        }
    }
    
}
