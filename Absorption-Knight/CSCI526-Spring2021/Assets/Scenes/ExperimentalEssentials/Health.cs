using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]int maxHealth;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "AttackZone")
        {
            currentHealth -= other.gameObject.GetComponent<AttackZone>().damage;
            Debug.Log("get hit!!! now HP " + currentHealth);
        }
    }
}
