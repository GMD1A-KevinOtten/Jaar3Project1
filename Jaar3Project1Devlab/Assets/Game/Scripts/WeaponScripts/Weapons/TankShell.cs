using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShell : MonoBehaviour {
    public float explosionRadius;
    public float minDist = 1;
    public float maxDist;

    private bool doneThing;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision col)
    {
        if (!doneThing)
        {
            doneThing = true;
            Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach (Collider c in cols)
            {
                if (c.GetComponentInParent<Soldier>())
                {
                    Debug.Log("Hit soldat");
                    float dist = Vector3.Distance(transform.position, c.transform.position);
                    float damage = GetComponent<Bullet>().defaultDamage;

                    if (dist <= minDist)
                    {
                        c.GetComponentInParent<Soldier>().TakeDamage(Mathf.RoundToInt(damage));
                    }
                    else
                    {

                        dist = Mathf.Clamp(dist, 0, maxDist);
                        float f = maxDist - minDist;
                        dist = dist / f;
                        float dmg = damage * dist;
                        dmg = damage - dmg;

                        c.GetComponentInParent<Soldier>().TakeDamage(Mathf.RoundToInt(dmg));
                    }

                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
      

    }
}
