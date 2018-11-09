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
        EffectsManager.instance.PlayAudio3D(EffectsManager.instance.FindAudioClip("BulletImpact Person"), gotHit.transform.position);
        Soldier soldier = gotHit.transform.root.GetComponent<Soldier>();
        soldier.TakeDamage(defaultDamage, gotHit.relativeVelocity);
		soldier.SetDamageOverTime(turns,poisenDamage);
        transform.SetParent(gotHit.transform);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public override void HitEnvironment(Collision gotHit)
    {
        transform.SetParent(gotHit.transform);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        string materialName = gotHit.transform.tag;

        switch (materialName)
        {
            case ("Sand"):
                EffectsManager.instance.PlayAudio3D(EffectsManager.instance.FindAudioClip("BulletImpact Sand"), gotHit.contacts[0].point);
                EffectsManager.instance.PlayParticle(EffectsManager.instance.FindParticle("BulletImpact Sand"), gotHit.contacts[0].point, gotHit.contacts[0].normal);
                EffectsManager.instance.CreateBulletHole(bulletHoleSprites, gotHit.contacts[0].point, Quaternion.FromToRotation(Vector3.forward, gotHit.contacts[0].normal), materialName);
                break;
            case ("Metal"):
                EffectsManager.instance.PlayAudio3D(EffectsManager.instance.FindAudioClip("BulletImpact Metal"), gotHit.contacts[0].point);
                EffectsManager.instance.PlayParticle(EffectsManager.instance.FindParticle("BulletImpact Metal"), gotHit.contacts[0].point, gotHit.contacts[0].normal);
                EffectsManager.instance.CreateBulletHole(bulletHoleSprites, gotHit.contacts[0].point, Quaternion.FromToRotation(Vector3.forward, gotHit.contacts[0].normal), materialName);
                break;
            case ("Wood"):
                EffectsManager.instance.PlayAudio3D(EffectsManager.instance.FindAudioClip("BulletImpact Wood"), gotHit.contacts[0].point);
                EffectsManager.instance.PlayParticle(EffectsManager.instance.FindParticle("BulletImpact Wood"), gotHit.contacts[0].point, gotHit.contacts[0].normal);
                EffectsManager.instance.CreateBulletHole(bulletHoleSprites, gotHit.contacts[0].point, Quaternion.FromToRotation(Vector3.forward, gotHit.contacts[0].normal), materialName);
                break;
            case ("Terrain"):
                EffectsManager.instance.PlayAudio3D(EffectsManager.instance.FindAudioClip("BulletImpact Sand"), gotHit.contacts[0].point);
                EffectsManager.instance.PlayParticle(EffectsManager.instance.FindParticle("BulletImpact Sand"), gotHit.contacts[0].point, gotHit.contacts[0].normal);
                break;
        }

        Destroy(gameObject);
    }
}
