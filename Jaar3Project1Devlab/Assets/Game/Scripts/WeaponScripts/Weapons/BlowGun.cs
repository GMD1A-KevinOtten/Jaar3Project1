using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowGun : Weapon {

    [Header("BlowGun Specific")]
    public Camera blowGunCam;
    public Vector3 originalRot;

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
        base.SpecialFunctionalityToggle();
        if(blowGunCam.GetComponent<Camera>().depth < Camera.main.depth)
        {
            //crosshair & camera
            blowGunCam.GetComponent<Camera>().depth = 1;
            UIManager.instance.showCroshair = false;
            UIManager.instance.HideCrosshair();

            //camera effects

        }
        else
        {
            blowGunCam.GetComponent<Camera>().depth = -1;
            UIManager.instance.showCroshair = true;
        }

    }
}
