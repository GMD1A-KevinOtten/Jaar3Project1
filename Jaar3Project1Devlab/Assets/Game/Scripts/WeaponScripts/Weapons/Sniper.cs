using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon {

    [Header("Sniper Specific")]

    public Camera cameraScope;
    public bool aiming;

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
            if(mySoldier.canShoot != true && mySoldier.canSwitch == true && !aiming)
            {
                if(currentClip != 0)
                {
                    mySoldier.CombatToggle();
                }
                
            }
        }
        if(Input.GetButtonDown("R"))
        {
            if(mySoldier.canShoot != true)
            {
                if(currentClip != clipMax)
                {
                    Reload();
                }
            }
        }
    }

    

    public override void SpecialFunctionalityToggle()
    {
        if(mySoldier.isActive)
        {
            base.SpecialFunctionalityToggle();
            UIManager.instance.ToggleScope();
            if(cameraScope.GetComponent<Camera>().depth < Camera.main.depth)
            {
                cameraScope.GetComponent<Camera>().depth = 1;
                UIManager.instance.showCroshair = false;
                UIManager.instance.HideCrosshair();
                aiming = true;
            }
            else
            {

                cameraScope.GetComponent<Camera>().depth = -1;
                UIManager.instance.showCroshair = true;
                aiming = false;
            }
            //Play sound
        }
    }
}
