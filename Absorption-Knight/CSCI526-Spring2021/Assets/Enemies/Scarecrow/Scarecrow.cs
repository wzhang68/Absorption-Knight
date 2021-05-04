using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : Enemy {
    protected override void Hit(Unit target, GameObject source, int damage) {
        throw new System.NotImplementedException();
    }

    protected override void OnHitBack(GameObject source, GameObject attacker, Vector2 hitBackDir, float hitBackSpeed) {
        base.OnHitBack(source, attacker, hitBackDir, hitBackSpeed);
    }

    protected override void InitializeActivateZones() {
        //do nothing
    }
}
