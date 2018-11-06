using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowGun : Weapon {

    [Header("BlowGun Specific")]
    public Camera blowGunCam;

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
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,2000));
        mySoldier.canShoot = false;
        mySoldier.currentWeaponIndex = 0;
        mySoldier.EquipWeapon();
        mySoldier.anim.SetBool("IsAiming",false);
        Invoke("BlowGunAfterAfterShot" , 2);
    }

    public void BlowGunAfterAfterShot()
    {
        SpecialFunctionalityToggle();
        Invoke("InvokeFunction" , 3);
    }

    public void InvokeFunction()
    {
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
        Destroy(this.gameObject);
    }
}
