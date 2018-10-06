using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDart : Bullet {

	public int poisenDamage;
	public int turns;

	public override void OnHit(Collision gotHit)
    { 
        HitSoldier(gotHit);
	}

	public override void HitSoldier(Collision gotHit)
    {
        Soldier soldier = gotHit.transform.root.GetComponent<Soldier>();
        soldier.TakeDamage(defaultDamage);
		soldier.SetDamageOverTime(turns,poisenDamage);
        Destroy(gameObject);
    }
}
