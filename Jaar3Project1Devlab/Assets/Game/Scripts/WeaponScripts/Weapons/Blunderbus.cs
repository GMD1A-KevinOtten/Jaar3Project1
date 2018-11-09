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
        if(mySoldier.isActive)
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
                if(mySoldier.canShoot != true && mySoldier.canSwitch == true)
                {
                    mySoldier.CombatToggle();
                }
            }
            if(Input.GetButtonDown("R"))
            {
                if(currentClip != clipMax)
                {
                    Reload();
                }
            }
        }
    }

    public IEnumerator ShootBlunderbus()
    {
        while(currentClip != 0)
        {
            ShootBullet();
        }
        Reload();
        yield return null;
    }

    public override IEnumerator AferReloadTeamSwitch(float time)
    {
        print("blunderbus ienum");
        yield return new WaitForSeconds(4);
        if(TeamManager.instance.mainCamera.cameraState != CameraMovement.CameraStates.Topview || TeamManager.instance.mainCamera.cameraState != CameraMovement.CameraStates.Idle)
        {
            mySoldier.CombatToggle();
            mySoldier.canSwitch = true;
            mySoldier.soldierMovement.canMove = true;
            TeamManager.instance.lastTeamIndex = TeamManager.instance.teamIndex;
            TeamManager.instance.NextTeam();
            hasShot = false;
        }
        else
        {
            yield return null;
            hasShot = false;
        }
    }
}
