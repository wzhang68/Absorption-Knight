using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    [SerializeField]public int damage;
    [SerializeField]public float damageTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        damageTime -= Time.deltaTime;
        if(damageTime <= 0.0f) {
            Destroy(this.gameObject);
        }
    }
}
