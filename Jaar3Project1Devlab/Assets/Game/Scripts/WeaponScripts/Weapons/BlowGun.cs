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
                BlowGunAfterShot();
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

    public void BlowGunAfterShot()
    {
        mySoldier.availableWeapons.Remove(this.gameObject);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponentInParent<IKControl>().activateIK = false;        
        gameObject.transform.SetParent(null);
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 2);
        SpecialFunctionalityToggle();
        mySoldier.canShoot = false;
        mySoldier.currentWeaponIndex = 0;
        mySoldier.EquipWeapon();
        mySoldier.anim.SetBool("IsAiming",false);
        Invoke("InvokeFunction" , 3);
    }

    public void InvokeFunction()
    {
        mySoldier.canShoot = true;
        TeamManager.instance.EndTheTurn();
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
