using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHPGauge : MonoBehaviour
{
    [SerializeField] public float maxHP;
    [SerializeField] public float currentHP;
    [SerializeField] public Vector2 HPGaugeOffset;
    [SerializeField] public Vector2 HPOriginScale;

    private Enemy thisEnemy;
    private float originLength;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponents<SpriteRenderer>()[0].enabled = true;
        this.gameObject.transform.localScale = new Vector3(HPOriginScale.x, HPOriginScale.y, 0);
        originLength = this.gameObject.transform.localScale.x;
        thisEnemy = this.gameObject.GetComponentInParent<Enemy>();
        maxHP = thisEnemy.getMaxHealth();
        currentHP = thisEnemy.getCurHealth();
    }

    // Update is called once per frame
    void Update()
    {
        currentHP = thisEnemy.getCurHealth();
        drawHpGauge();
    }

    void drawHpGauge(){
        if(currentHP == 0) {
            StopRenderer();
            return;
        }
        this.transform.position = thisEnemy.transform.position + new Vector3(HPGaugeOffset.x, HPGaugeOffset.y, 0);
        Vector3 temp = this.transform.localScale;
        temp.x = originLength * (currentHP / maxHP);
        this.transform.localScale = temp;
    }

    void StopRenderer(){
        this.gameObject.GetComponents<SpriteRenderer>()[0].enabled = false;
    }
}
