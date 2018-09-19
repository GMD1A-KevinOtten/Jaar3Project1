using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int defaultDamage;

    private void OnCollisionEnter(Collision collision)
    {
        OnHit(collision.transform);
    }

    public virtual void OnHit(Transform gotHit)
    {
        if (gotHit.tag == "Soldier")
        {
            print("Soldier");
            Soldier soldier = gotHit.GetComponent<Soldier>();
            soldier.TakeDamage(defaultDamage);
            Destroy(gameObject);
        }
    }


}
