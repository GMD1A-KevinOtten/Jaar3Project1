using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blunderbus : Weapon 
{
    private bool hasShot;

    public override void Update() 
    {
        base.Update();
    }

    public override void Inputs()
    {
        if(mySoldier.canShoot == true)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if(hasShot == false && currentClip != 0)
                {
                    mySoldier.anim.SetTrigger("Shoot");
                    hasShot = true;
                    StartCoroutine(ShootBlunderbus());
                }
                
            }
        }   
        if(Input.GetButtonDown("Fire2"))
        {
            if(mySoldier.canShoot != true)
            {
                mySoldier.CombatToggle();
            }
        }
        if(Input.GetButtonDown("R"))
        {
            Reload(0);
        }
    }

    public IEnumerator ShootBlunderbus()
    {
        while(currentClip != 0)
        {
            ShootBullet();
        }
        hasShot = false;
        yield return null;
    }
}
