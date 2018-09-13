using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {

    public int health;
    public bool isDead;
    public bool isActive;
    
    public List<Weapon> availableWeapons = new List<Weapon>();
    public Weapon equippedWeapon;

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            ShootWeapon();
        }
    }

    public void TakeDamage(int toDamage)
    {
        health -= toDamage;

        if (health <= 0)
        {
            isDead = true;
        }
    }

    public void EquipWeapon(int weaponIndex)
    {
        equippedWeapon = availableWeapons[weaponIndex];
        //Equip weapon animation is played
    }

    public void ShootWeapon()
    {
        equippedWeapon.ShootBullet();
    }
}
