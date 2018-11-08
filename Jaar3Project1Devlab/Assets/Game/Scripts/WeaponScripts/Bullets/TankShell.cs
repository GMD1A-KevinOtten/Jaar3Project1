using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShell : Bullet {
    public float explosionRadius;
    public float minDist = 1;
    public float maxDist;
	
    public override void OnHit(Collision gotHit)
    {
        TankShellEffect(gotHit);
    }

	void OnDrawGizmos () 
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }

    private void TankShellEffect(Collision col)
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
                if (dist <= minDist)
                {
                    g.GetComponent<Soldier>().TakeDamage(Mathf.RoundToInt(defaultDamage), new Vector3(0,0,0));
                }
                else if (dist < maxDist && dist > minDist)
                {
                    dist = Mathf.Clamp(dist, minDist, maxDist);
                    dist -= minDist;
                    float f = maxDist - minDist;
                    dist = dist / f;


                    float dmg = defaultDamage * dist;
                    dmg = defaultDamage - dmg;

                    g.GetComponent<Soldier>().TakeDamage(Mathf.RoundToInt(dmg), new Vector3(0,0,0));
                }

            }
        }

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision col)
    {
        EffectsManager.instance.PlayAudio3D(EffectsManager.instance.FindAudioClip("Explosion01"), transform.position, 30);
        EffectsManager.instance.PlayParticle(EffectsManager.instance.FindParticle("Explosion01"), transform.position, Vector3.up);
        TankShellEffect(col);
    }
}