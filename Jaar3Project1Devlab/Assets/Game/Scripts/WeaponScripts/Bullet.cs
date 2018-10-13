using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Sprite bulletHoleSprite;
    public GameObject bulletHolePrefab;
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
        Soldier soldier = gotHit.transform.root.GetComponent<Soldier>();
        soldier.TakeDamage(defaultDamage, gotHit.relativeVelocity);
        Destroy(gameObject);
    }

    public virtual void HitEnvironment(Collision gotHit)
    {
        GameObject bulletHoleObject = Instantiate(bulletHolePrefab, gotHit.contacts[0].point, Quaternion.FromToRotation(Vector3.forward, gotHit.contacts[0].normal));
        bulletHoleObject.gameObject.name = "BulletHole";
        bulletHoleObject.transform.position = bulletHoleObject.transform.localPosition + bulletHoleObject.transform.forward * 0.001f;
        SpriteRenderer sr = bulletHoleObject.GetComponent<SpriteRenderer>();
        sr.sprite = bulletHoleSprite;
        Destroy(gameObject);
    }

}
