using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueFollow : MonoBehaviour
{
    private GameObject dialoguebox = null;
    private Text dialoguetext;
    private Camera camera;
    private GameObject npc;
    private GameObject Teodoro;
    private Vector3 offset = new Vector3(-200,350,0);
    private GameObject[] npcs;
    
    void Start()
    {
        if (GameObject.Find("DialogueManager") != null)
        {
            dialoguebox = GameObject.Find("DialogueUIPanel_Blue");
            camera = FindObjectOfType<Camera>();
            npc = GameObject.FindGameObjectWithTag("npc");
            Teodoro = GameObject.Find("Teodoro_ZLVer");
            // Teodoro = GameObject.Find("Teodoro_ZLVer(Clone)");
            npcs = GameObject.Find("DialogueManager").GetComponent<NpcContainer>().NPCs;
        }
    }

    private void Update()
    {
        if (dialoguebox != null)
        {
            dialoguebox.transform.position = WorldPos2Rect(camera, GetClosestNPC().transform.position) + offset;
        }

    }
    
    
    public static Vector3 WorldPos2Rect(Camera camera,Vector3 worldPos)
    {
        return camera.WorldToScreenPoint(worldPos); 
    }

    private GameObject GetClosestNPC()
    {
        GameObject betTarget = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject npc in npcs)
        {
            Vector3 directionToTarget = npc.transform.position - Teodoro.transform.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistance)
            {
                closestDistance = dSqrToTarget;
                betTarget = npc;
            }
        }
        
        return betTarget;
    }
    
    
}
