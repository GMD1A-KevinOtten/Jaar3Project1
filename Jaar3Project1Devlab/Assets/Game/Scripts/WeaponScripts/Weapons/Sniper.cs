using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon {

    [Header("Sniper Specific")]

    public Camera cameraScope;

    public override void Start() 
    {
        base.Start();
    }

	public override void  Update() 
    {
        base.Update();
    }

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
            print("RightKlick");
            if(mySoldier.canShoot != true)
            {
                mySoldier.CombatToggle();
            }
            else
            {
                SpecialFunctionalityToggle();
            }
        }
    }

    

    public override void SpecialFunctionalityToggle()
    {
        base.SpecialFunctionalityToggle();
        UIManager.instance.ToggleScope();
        if(cameraScope.GetComponent<Camera>().depth < Camera.main.depth)
        {
            cameraScope.GetComponent<Camera>().depth = 1;
            UIManager.instance.showCroshair = false;
            UIManager.instance.HideCrosshair();
        }
        else
        {
            cameraScope.GetComponent<Camera>().depth = -1;
            UIManager.instance.showCroshair = true;
        }
        
        //Scope UI element
        //Play sound
    }
}
