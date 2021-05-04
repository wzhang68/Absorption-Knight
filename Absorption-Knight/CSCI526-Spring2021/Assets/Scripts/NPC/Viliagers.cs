using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Viliagers : MonoBehaviour
{
    [SerializeField] private float min_range;
    [SerializeField] private float max_range;
    public float duration;    //the max time of a walking session (set to ten)
    float elapsedTime   = 0f; //time since started walk
    float wait          = 0f; //wait this much time
    float waitTime      = 0f; //waited this much time

    float randomX;  //randomly go this X direction

    bool move = true; //start moving
    bool stopwander = false;
    private Animator anim;
    private DialogueTrigger m_trigger;
    
    
    void Start(){
        randomX =  Random.Range(-1f,1f);
    }
    


    void Awake()
    {
        anim = GetComponent<Animator>();
        m_trigger = GetComponent<DialogueTrigger>();
    }
    
    
    /// <summary>
    /// This is the script for NPC wandering 
    /// </summary>
    void Update()
    {
        if (randomX < 0)
        {
            transform.localScale =new Vector3(-1f, 1f, 1f);
        }
        else if (randomX == 0)
        {
            anim.SetBool("walkable",false);
        }
        else
        {
            transform.localScale =new Vector3(1f, 1f, 1f);
        }

        if (m_trigger.walk)
        {
            if (transform.position.x <= min_range)
            {
                randomX = Random.Range(0f, 1f);
            }
            else if (transform.position.x >= max_range)
            {
                randomX = Random.Range(-1f, 0f);
            }
        }

        if (!stopwander)
        {
            if (elapsedTime < duration && move)
            {
                //if its moving and didn't move too much
                anim.SetBool("walkable", true);
                transform.Translate(new Vector2(randomX, 0) * Time.deltaTime);
                elapsedTime += Time.deltaTime;

            }
            else
            {
                //do not move and start waiting for random time
                anim.SetBool("walkable", false);
                move = false;
                wait = Random.Range(5, 7);
                waitTime = 0f;
                while (waitTime < wait)
                {
                    waitTime += Time.deltaTime;
                }

                move = true;
                elapsedTime = 0f;
                randomX = Random.Range(-1f, 1f);
            }
        }
    }

    public void StopWandering()
    {
        stopwander = true;

    }

    public void Wandering()
    {
        stopwander = false;
    }


}
