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
        if (isTank)
        {
            if (transform.root.GetComponent<Tank>().soldierInside)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    ShootBullet();
                }
            }
        
        }
        
    }
}
