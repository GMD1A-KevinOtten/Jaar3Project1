using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDart : Bullet {

	public int poisenDamage;
	public int turns;

	public override void OnHit(Collision gotHit)
    { 
        switch (gotHit.gameObject.layer)
        {
            case 10:
                HitSoldier(gotHit);
                break;
            case 12:
                HitEnvironment(gotHit);
                break;
        }
	}

	public override void HitSoldier(Collision gotHit)
    {
        Soldier soldier = gotHit.transform.root.GetComponent<Soldier>();
        soldier.TakeDamage(defaultDamage);
		soldier.SetDamageOverTime(turns,poisenDamage);
        transform.SetParent(gotHit.transform);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public override void HitEnvironment(Collision gotHit)
    {
        print("envior");
        transform.SetParent(gotHit.transform);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}
