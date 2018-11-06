using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Makarov : Weapon {


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
            if(mySoldier.canShoot != true)
            {
                mySoldier.CombatToggle();
                TeamManager.instance.mainCamera.cameraState = CameraMovement.CameraStates.CombatVieuw;
            }
        }
    }

     public override void ShootBullet()
    {
        if(currentClip > 0)
        {
            EffectsManager.instance.PlayAudio3D(shotSound,transform.position);
            EffectsManager.instance.PlayParticle(EffectsManager.instance.FindParticle("Muzzle Flash"), barrelExit.position, barrelExit.forward);
            mySoldier.anim.SetTrigger("Shoot");

            newGameObject = Instantiate(bulletPrefab, barrelExit.position, barrelExit.rotation);
            currentClip -= 1;
                    
            Rigidbody rb = newGameObject.GetComponent<Rigidbody>();
            Vector2 spread = CalculatedBulletSpread();

            Vector3 bulletDirection = barrelExit.forward * bulletVelocity;
            bulletDirection.x += spread.x;
            bulletDirection.y += spread.y;
            rb.velocity = bulletDirection;
        }
        else
        {
            EffectsManager.instance.PlayAudio3D(EffectsManager.instance.FindAudioClip("Empty Clip"), transform.position);
            Reload();
        }
    }

    public override void Reload()
    {
        mySoldier.canShoot = false;
        FillClip();
        mySoldier.anim.SetTrigger("Reload");
    }

    public override void SpecialFunctionalityToggle()
    {
        mySoldier.canShoot = true;
    }
}
