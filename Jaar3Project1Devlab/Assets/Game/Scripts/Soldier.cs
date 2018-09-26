using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {

    /// <summary>
    /// PlayerCamPos is always the first child of the object that contains the Soldier Class
    /// </summary>
    public Transform thirdPersonCamPos;
    public Transform weaponSpawnLocation;
    public Movement soldierMovement;

    [Header("Activity Proporties")]
    public int health;
    public bool isDead;
    public bool isActive;
    
    public List<GameObject> availableWeaponsPrefabs = new List<GameObject>();
    [HideInInspector]
    public Weapon equippedWeapon;

    void Start()
    {
        soldierMovement = GetComponent<Movement>();
        EquipWeapon(0);
    }

    private void Update()
    {
        if (isActive)
        {
            if(equippedWeapon != null)
            {
                equippedWeapon.ShowCrosshair();

                if (NWManager.instance.playingMultiplayer)
                {
                    if(TeamManager.instance.currentPlayer == PhotonNetwork.player)
                    {
                        if (Input.GetButtonDown("Fire1"))
                        {
                            ShootWeapon();
                        }
                    }
                }
                else
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        ShootWeapon();
                    }
                }
               
            }
        }

    }

    public void TakeDamage(int toDamage)
    {
        print("0");
        health -= toDamage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void EquipWeapon(int weaponIndex)
    {
        if(equippedWeapon != null)
        {
            Destroy(equippedWeapon.gameObject);
        }
        //uitvoeren op het punt waar wapen moet verschijnen
        GameObject equippedWeaponObject = InstantiateWeapon(weaponIndex);
        equippedWeapon = equippedWeaponObject.GetComponent<Weapon>();
    }

    public GameObject InstantiateWeapon(int weaponIndex)
    {
        GameObject currenWeapon = Instantiate(availableWeaponsPrefabs[weaponIndex], weaponSpawnLocation.position,weaponSpawnLocation.rotation);
        currenWeapon.transform.SetParent(weaponSpawnLocation);
        currenWeapon.GetComponent<Rigidbody>().isKinematic = true;
        return currenWeapon;
    }

    public void ShootWeapon()
    {
        if (NWManager.instance.playingMultiplayer)
        {
            if (TeamManager.instance.currentPlayer == PhotonNetwork.player)
            {
                equippedWeapon.photonView.RPC("ShootBullet", PhotonTargets.All);
            }
        }
        else
        {
            equippedWeapon.ShootBullet();
        }
    }

    public void Die()
    {
        isDead = true;
        TeamManager.instance.allTeams[TeamManager.instance.teamIndex].CheckTeam();

        //speel animatie af
    }

    public void switchGuns()
    {
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {

        }

         if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {

        }
    }
}
