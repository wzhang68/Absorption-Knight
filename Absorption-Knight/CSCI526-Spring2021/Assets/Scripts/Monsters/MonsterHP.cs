using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHP : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    public int currHp;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        currHp = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currHp <= 0)
        {
            anim.SetTrigger("die");
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "AttackZone")
        {
            currHp -= other.gameObject.GetComponent<AttackZone>().damage;
            anim.SetTrigger("hit");
            Debug.Log("get hit!!! now HP " + currHp);
        }
    }
}
