using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Makarov : Weapon {


    public override void Inputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(mySoldier.canShoot == true)
            {
                ShootBullet();
            }
        }
        if(Input.GetButtonDown("Fire2"))
        {
            if(mySoldier.canShoot != true)
            {
                mySoldier.CombatToggle();
                SpecialFunctionalityToggle();
            }
        }
    }
    
   public override void SpecialFunctionalityToggle()
   {
        Reload();
   }
}
