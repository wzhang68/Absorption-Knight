using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLurker : Enemy{
    [SerializeField] private float m_hitBackSpeed = 1f;
    [SerializeField] protected int m_damagePerHit = 0;

    protected override void Start() {
        base.Start();
        m_destination = m_target;
    }

    protected override void Hit(Unit hitUnit, GameObject source, int damage) {
        //no self hit back
        //Vector3 hitBackDir = hitUnit.transform.position - transform.position; //连线方向做为方向

        // hitbackdir 为player的反方向
        //Vector2 hitBackDir;
        //if (hitUnit.transform.localScale.x > 0.1f){
        //    hitBackDir = new Vector2(-1f, 1f);
        //}else {
        //    hitBackDir = new Vector2(1f, 1f);
        //}

        //固定右上或者左上为方向
        Vector2 hitBackDir = new Vector2(hitUnit.transform.position.x >= transform.position.x ? 1f : -1f, 1f);

        hitBackDir.Normalize();
        hitUnit.OnHit(this.gameObject, source, damage, hitBackDir, m_hitBackSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit != null) {
            Hit(unit, this.gameObject, m_damagePerHit);
        }
    }

    protected override void OnHitBack(GameObject source, GameObject attacker, Vector2 dir, float hitBackSpeed) {
        //Lurker will not be hit back, so override and do nothing
        //base.OnHitBack(source, attacker, dir, hitBackSpeed);
    }

    public override void OnHit(GameObject source, GameObject attacker, int damage, Vector2 hitBackDir, float hitBackSpeed) {
        //Lurker will not be hit, so override and do nothing
        //base.OnHit(source, attacker, damage, hitBackDir, hitBackSpeed);
    }

    public override void OnHeal(GameObject source, GameObject healer, int value) {
        //base.OnHeal(source, healer, value);
    }

    protected override void InitializeActivateZones() {
        //do nothing
    }
}
