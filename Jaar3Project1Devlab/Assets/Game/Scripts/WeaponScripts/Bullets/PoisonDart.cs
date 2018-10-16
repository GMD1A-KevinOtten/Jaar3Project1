using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDart : Bullet {

	public int poisenDamage;
	public int turns;
    public bool hitTarget;

	public override void OnHit(Collision gotHit)
    { 
        if(hitTarget != true)
        {
            hitTarget = true;
            switch (gotHit.gameObject.layer)
            {
                case 13:
                print("soldier");
                    HitSoldier(gotHit);
                    break;
                case 12:
                print("enviorment");
                    HitEnvironment(gotHit);
                    break;
            }
            gameObject.GetComponent<Collider>().enabled = false;
        }  
	}

	public override void HitSoldier(Collision gotHit)
    {
        EffectsManager.instance.PlayAudio3D(EffectsManager.instance.FindAudioClip("PlaceHolder"), gotHit.transform.position);
        Soldier soldier = gotHit.transform.root.GetComponent<Soldier>();
        soldier.TakeDamage(defaultDamage, gotHit.relativeVelocity);
		soldier.SetDamageOverTime(turns,poisenDamage);
        transform.SetParent(gotHit.transform);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public override void HitEnvironment(Collision gotHit)
    {
        EffectsManager.instance.PlayAudio3D(EffectsManager.instance.FindAudioClip("PlaceHolder"), gotHit.transform.position);
        transform.SetParent(gotHit.transform);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}
