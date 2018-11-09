using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TommyGun : Weapon {

    [Header("TommyGun Specifics")]
    public float rpm;
    private bool canShoot = true;
    public GameObject magazine;
    private Vector3 positionMagazine;
    private bool step1;

    public override void Start() 
    {
        base.Start();
        magazine = transform.GetChild(1).gameObject;
        positionMagazine = magazine.transform.localPosition;
    }

	public override void Update() 
    {
        base.Update();
    }

    public override void Inputs()
    {
        if(mySoldier.isActive)
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
                if(mySoldier.canShoot != true && mySoldier.canSwitch == true)
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
                        magazine.transform.parent = null;
                        magazine.GetComponent<Rigidbody>().isKinematic = false;
                    }
                }
            }
        }
    }

    public override void SpecialFunctionalityToggle()
    {
        if(!step1)
        {
            magazine.transform.SetParent(mySoldier.leftHand.transform);
            magazine.GetComponent<Rigidbody>().isKinematic = true;
            magazine.transform.localPosition = Vector3.zero;
            magazine.transform.localEulerAngles = new Vector3(-90,0,0);
            step1 = true;
        }
        else
        {
            magazine.transform.SetParent(mySoldier.equippedWeapon.gameObject.transform);
            magazine.transform.localPosition = positionMagazine;
            magazine.transform.localEulerAngles = Vector3.zero;
            step1 = false;
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
