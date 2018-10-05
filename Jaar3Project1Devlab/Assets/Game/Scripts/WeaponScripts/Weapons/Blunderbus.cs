using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blunderbus : Weapon 
{

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
                StartCoroutine(ShootBlunderbus());
            }
        }   
        if(Input.GetButtonDown("Fire2"))
        {
            print("RightKlick");
            if(mySoldier.canShoot != true)
            {
                mySoldier.CombatToggle();
            }
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
