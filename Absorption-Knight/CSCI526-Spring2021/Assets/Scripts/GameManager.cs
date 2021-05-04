using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameObject player;
    public static string prevScene;
    public GameObject HPGauge;

    private void Awake()
    {
        Debug.Log("gm initialized");
        if (instance == null)
        {
            instance = this;
        }else if (instance != this)
        {
            Destroy(gameObject);
        }

        if (GameObject.Find("Teodoro_ZLVer")==null)
        {
            GameObject go = Instantiate(player, new Vector3(0f, 0f, 0f),Quaternion.identity);
            go.name = go.name.Replace("(Clone)", "");
            if (File.Exists(Application.persistentDataPath + "/playerData.txt"))
            {
                go.GetComponent<PlayerUnit>().LoadPlayer();
            }
            else
            {
                go.GetComponent<PlayerUnit>().SavePlayer();
                go.GetComponent<PlayerUnit>().LoadPlayer();
            }
        }
        else
        {
            gameObject.GetComponent<PlayerUnit>().LoadPlayer();
        }

    }
    
}
