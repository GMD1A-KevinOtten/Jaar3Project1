using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {

    /// <summary>
    /// PlayerCamPos is always the first child of the object that contains the Soldier Class
    /// </summary>
    public Transform playerCamPos;
    public Movement soldierMovement;

    public int health;
    public bool isDead;
    public bool isActive;
    
    public List<Weapon> availableWeapons = new List<Weapon>();
    public Weapon equippedWeapon;

    void Start()
    {
        playerCamPos = transform.GetChild(0).transform;
        soldierMovement = gameObject.GetComponent<Movement>();
    }

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
