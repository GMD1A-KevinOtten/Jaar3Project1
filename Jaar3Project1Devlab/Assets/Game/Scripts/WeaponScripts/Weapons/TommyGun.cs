using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TommyGun : Weapon {

    [Header("TommyGun Specifics")]
    public float rpm;
    private bool canShoot = true;

    public override void Start() 
    {
        base.Start();
    }

	public override void Update() 
    {
        if(gameObject.transform.root.GetComponent<Soldier>() != null)
        {
            if(gameObject.transform.root.GetComponent<Soldier>().isActive == true)
            {
                Inputs();
            }
        }
    }

    public override void Inputs()
    {
        if(canShoot)
        {
            if(mySoldier.canShoot == true)
            {
                if (Input.GetButton("Fire1"))
                {
                    canShoot = false;
                    StartCoroutine(ShotWaitTime());
                }
            }
        }
        if(Input.GetButtonDown("Fire2"))
        {
            print("RightKlick");
            if(mySoldier.canShoot != true)
            {
                mySoldier.CombatToggle();
            }
        }
    }

    public float NextShotWaitCalculation()
    {
        float waitTime;

        waitTime = 60 / rpm;

        return waitTime;
    }

    public IEnumerator ShotWaitTime()
    {
        yield return new WaitForSeconds(NextShotWaitCalculation());
        canShoot = true;
        ShootBullet();
    }
}
