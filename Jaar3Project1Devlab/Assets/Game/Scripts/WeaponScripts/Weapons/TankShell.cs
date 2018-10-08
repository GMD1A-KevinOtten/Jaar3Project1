using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShell : MonoBehaviour {
    public float explosionRadius;
    public float minDist = 1;
    public float maxDist;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnDrawGizmos () {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, explosionRadius);

    }

    private void Update()
    {
      
    }

    private void OnCollisionEnter(Collision col)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);
        List<GameObject> objs = new List<GameObject>();
        foreach (Collider c in cols)
        {
            if (!objs.Contains(c.transform.root.gameObject))
            {
                objs.Add(c.transform.root.gameObject);
            }
        }

        foreach (GameObject g in objs)
        {
            if (g.GetComponent<Soldier>())
            {

                float dist = Vector3.Distance(transform.position, g.transform.position);
                float damage = GetComponent<Bullet>().defaultDamage;
                if (dist <= minDist)
                {
                    g.GetComponent<Soldier>().TakeDamage(Mathf.RoundToInt(damage));
                }
                else if (dist < maxDist && dist > minDist)
                {
                    dist = Mathf.Clamp(dist, minDist, maxDist);
                    dist -= minDist;
                    float f = maxDist - minDist;
                    dist = dist / f;


                    float dmg = damage * dist;
                    dmg = damage - dmg;

                    g.GetComponent<Soldier>().TakeDamage(Mathf.RoundToInt(dmg));
                }

            }
        }

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Destroy(gameObject);
    }
}