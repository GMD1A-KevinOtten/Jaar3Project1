using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Weapon {

    private void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            ShootBullet();
        }
    }
}
