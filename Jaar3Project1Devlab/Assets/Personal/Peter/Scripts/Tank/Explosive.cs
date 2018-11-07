using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Weapon {
    private bool exiting;

    public override void Start()
    {
        base.Start();
    }

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
        if (isTank)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (transform.root.GetComponent<Tank>().soldierInside && mySoldier.canShoot)
                {
                    ShootBullet();
                    if (!exiting)
                    {
                        StartCoroutine(ExitEnd());
                    }
                }
            }
        }
        else if (!isTank)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (mySoldier != null)
                {
                    if (mySoldier.canShoot && currentClip != 0)
                    {
                        ShootBullet();
                        GrenadeLauncherAfterShot();
                    }
                }

            }
            if (Input.GetButtonDown("Fire2"))
            {
                print("RightKlick");
                if (mySoldier.canShoot != true)
                {
                    mySoldier.CombatToggle();
                }
            }
        }
    }

    public void GrenadeLauncherAfterShot()
    {
        mySoldier.availableWeapons.Remove(this.gameObject);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponentInParent<IKControl>().activateIK = false;        
        gameObject.transform.SetParent(null);
        GetComponent<Rigidbody>().AddForce(Vector3.back * 2);
        mySoldier.canShoot = false;
        mySoldier.currentWeaponIndex = 0;
        mySoldier.EquipWeapon();
        mySoldier.canSwitch = false;
        mySoldier.anim.SetBool("IsAiming",false);
        Invoke("InvokeFunction" , 3);
    }

    public void InvokeFunction()
    {
        if(TeamManager.instance.mainCamera.cameraState != CameraMovement.CameraStates.Idle || TeamManager.instance.mainCamera.cameraState != CameraMovement.CameraStates.Topview)
        {
            mySoldier.canShoot = true;
            TeamManager.instance.EndTheTurn();
        }
        Invoke("SelfDestruct", 3);
    }

    private void SelfDestruct()
    {
        mySoldier.canSwitch = true;   
        print("Explosive");
        Destroy(this.gameObject);
    }

    public override void SpecialFunctionalityToggle()
    {
        base.SpecialFunctionalityToggle();
        if (GetComponentInChildren<Camera>().depth < Camera.main.depth)
        {
            GetComponentInChildren<Camera>().depth = 1;
            Camera.main.depth = -1;
            UIManager.instance.showCroshair = false;
            UIManager.instance.HideCrosshair();
        }
        else
        {
            GetComponentInChildren<Camera>().depth = -1;
            Camera.main.depth = 1;
            UIManager.instance.showCroshair = true;
        }

  
    }

    private IEnumerator ExitEnd()
    {
        exiting = true;
        yield return new WaitForSeconds(2);
        transform.root.GetComponent<Tank>().ExitTank();
        TeamManager.instance.EndTheTurn();

        exiting = false;


    }
}
