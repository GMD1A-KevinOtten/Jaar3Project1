using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon {

    [Header("Sniper Specific")]

    public Camera cameraScope;
    public bool aiming;
    public bool alreadyShot;
    public float timeToHoldBreathMax;
    public float timeToHoldBreathcurrent;
    public float holdBreathSpeed;

    public override void Start() 
    {
        base.Start();
        timeToHoldBreathcurrent = timeToHoldBreathMax;
    }

    public override void Inputs()
    {
        if(mySoldier.isActive)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if(mySoldier.canShoot == true && alreadyShot == false)
                {
                    ShootBullet();
                    alreadyShot = true;
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
            if(Input.GetButton("Shift") && aiming)
            {
                SlowAimingAniamtion();
            }
            if(Input.GetButtonUp("Shift") && aiming)
            {
                ResetAimingAnimation();
            }
        }
    }

    public void SlowAimingAniamtion()
    {
        if(timeToHoldBreathcurrent > 0)
        {
            timeToHoldBreathcurrent -= Time.deltaTime;
            mySoldier.anim.SetFloat("SniperHoldBreath", holdBreathSpeed);
        }
        else if(mySoldier.anim.GetFloat("SniperHoldBreath") != 1)
        {
            mySoldier.anim.SetFloat("SniperHoldBreath", 1);
        }
    }
    public void ResetAimingAnimation()
    {
        if(mySoldier.anim.GetFloat("SniperHoldBreath") != 1)
        {
            mySoldier.anim.SetFloat("SniperHoldBreath", 1);
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
                alreadyShot = false;
                timeToHoldBreathcurrent = timeToHoldBreathMax;
                ResetAimingAnimation();
            }
            //Play sound
        }
    }
}
