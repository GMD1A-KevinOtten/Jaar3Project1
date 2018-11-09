using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Sprite[] bulletHoleSprites;
    public int defaultDamage;

    private void OnCollisionEnter(Collision collision)
    {
        OnHit(collision);
    }

    public virtual void OnHit(Collision gotHit)
    {
        switch (gotHit.gameObject.layer)
        {
            case 13:
                HitSoldier(gotHit);
                break;
            case 12:
                HitEnvironment(gotHit);
                break;
        }
    }

    public virtual void HitSoldier(Collision gotHit)
    {
        EffectsManager.instance.PlayAudio3D(EffectsManager.instance.FindAudioClip("BulletImpact Person"), gotHit.transform.position);
        EffectsManager.instance.PlayParticle(EffectsManager.instance.FindParticle("BulletImpact Person"), gotHit.contacts[0].point, gotHit.contacts[0].normal);

        print(gotHit.transform.gameObject);
        Soldier soldier = gotHit.transform.root.GetComponent<Soldier>();
        soldier.hitPosition = gotHit.contacts[0];
        soldier.hitBone = gotHit.gameObject;
        soldier.TakeDamage(defaultDamage, gotHit.relativeVelocity);
        Destroy(gameObject);
    }

    public virtual void HitEnvironment(Collision gotHit)
    {
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
