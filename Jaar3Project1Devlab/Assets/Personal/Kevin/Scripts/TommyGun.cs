using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TommyGun : Weapon {

    [Header("TommyGun Specifics")]
    public float rpm;


	void Update() 
    {
        if(gameObject.transform.root.GetComponent<Soldier>() != null)
        {
            if(gameObject.transform.root.GetComponent<Soldier>().isActive == true)
            {
                
            }
        }
    }

    public float NextShotWaitCalculation()
    {
        float waitTime;

        waitTime = 60 / rpm;

        return waitTime;
    }
}
