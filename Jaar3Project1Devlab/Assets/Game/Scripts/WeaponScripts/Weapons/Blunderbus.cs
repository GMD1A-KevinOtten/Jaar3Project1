using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blunderbus : Weapon 
{

    public override void Update() 
    {
        if(gameObject.transform.root.GetComponent<Soldier>() != null)
        {
            if(transform.root.GetComponent<Soldier>().isActive == true)
            {
                Inputs();
            }
        }
    }

    public override void Inputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(ShootBlunderbus());
        }
    }

    public IEnumerator ShootBlunderbus()
    {
        while(currentClip != 0)
        {
            ShootBullet();
        }
        yield return null;
    }
}
