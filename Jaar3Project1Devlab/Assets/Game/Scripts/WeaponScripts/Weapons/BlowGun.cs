using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowGun : Weapon {

    [Header("BlowGun Specific")]
    public Camera blowGunCam;
    public bool shot;

    public override void Update()
    {
        base.Update();
        if(mySoldier != null)
        {
            if(mySoldier.canShoot == false)
            {
                if(TeamManager.instance.combatTurnTime <= 0.1f)
                {
                    mySoldier.canShoot = true;
                }
            }
        }
    }

    public override void Inputs()
    {
        if(mySoldier.isActive)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if(mySoldier.canShoot == true)
                {
                    shot = true;
                    ShootBullet();
                    BlowGunAfterShot();
                }
            }
            if(Input.GetButtonDown("Fire2"))
            {
                if(mySoldier.canShoot == false && !shot && mySoldier.canSwitch)
                {
                    mySoldier.CombatToggle();
                    SpecialFunctionalityToggle();
                }
            }
        }
    }

    public void BlowGunAfterShot()
    {
        mySoldier.availableWeapons.Remove(this.gameObject);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        if(GetComponentInParent<IKControl>() != null)
        {
            GetComponentInParent<IKControl>().activateIK = false;
        }
        gameObject.transform.SetParent(null);
        blowGunCam.transform.SetParent(null);
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0,500));
        mySoldier.canShoot = false;
        mySoldier.currentWeaponIndex = 0;
        mySoldier.EquipWeapon();
        mySoldier.canSwitch = false;
        print("switch 2");
        mySoldier.anim.SetBool("IsAiming",false);
        Invoke("InvokeFunction" , 2);
    }

    public void InvokeFunction()
    {
        print("test2");
        SpecialFunctionalityToggle();

        if(TeamManager.instance.mainCamera.cameraState != CameraMovement.CameraStates.Idle || TeamManager.instance.mainCamera.cameraState != CameraMovement.CameraStates.Topview)
        {
            mySoldier.canShoot = true;
            TeamManager.instance.EndTheTurn();  
        }
        Invoke("SelfDestruct",3);
    }

	public override void SpecialFunctionalityToggle()
    {
        if(mySoldier.isActive)
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

    private void SelfDestruct()
    {
        mySoldier.canSwitch = true;
        mySoldier.canShoot = false;
        Destroy(blowGunCam.gameObject);
        Destroy(this.gameObject);
    }
}
