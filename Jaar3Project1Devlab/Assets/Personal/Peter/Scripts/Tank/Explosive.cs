using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Weapon {


    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
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
                }
            }
        }
        else if (!isTank)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (mySoldier != null)
                {
                    if (mySoldier.canShoot)
                    {
                        ShootBullet();
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
}
