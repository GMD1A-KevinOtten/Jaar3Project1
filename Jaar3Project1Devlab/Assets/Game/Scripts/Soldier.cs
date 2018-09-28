using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Photon.PunBehaviour {

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
    
    [Header("Weapon propertys")]
    public List<GameObject> availableWeaponsPrefabs = new List<GameObject>();
    [HideInInspector]
    public Weapon equippedWeapon;
    public int currentWeaponIndex;
    private bool canSwitch = true;

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
            CheckScroll();
        }
    }

    [PunRPC]
    public void TakeDamage(int toDamage)
    {
        if(isDead == false)
        {
            health -= toDamage;

            if (health <= 0)
            {
                if (NWManager.instance.playingMultiplayer)
                {
                    photonView.RPC("Die", PhotonTargets.All);
                }
                else
                {
                    Die();
                }
            }
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
        if(!NWManager.instance.playingMultiplayer){
            GameObject currenWeapon = Instantiate(availableWeaponsPrefabs[weaponIndex], weaponSpawnLocation.position,weaponSpawnLocation.rotation);
            currenWeapon.transform.SetParent(weaponSpawnLocation);
            currenWeapon.GetComponent<Rigidbody>().isKinematic = true;
            return currenWeapon;
        }
        else
        {
            GameObject currenWeapon = PhotonNetwork.Instantiate("PRE_SniperRifle", weaponSpawnLocation.position,weaponSpawnLocation.rotation,0);
            currenWeapon.transform.SetParent(weaponSpawnLocation);
            currenWeapon.GetComponent<Rigidbody>().isKinematic = true;
            return currenWeapon;
        }
       
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

    [PunRPC]
    public void Die()
    {
        isDead = true;
        foreach (Team team in TeamManager.instance.allTeams)
        {
            if(team.SoldierCheck(this) == true)
            {
                break;
            }
        }
        transform.Rotate(new Vector3(90,0,0),Space.Self);
        //speel animatie af
    }

    public void CheckScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
         if (canSwitch)
        {
            canSwitch = false;
            
            int previouseWeaponIndex = currentWeaponIndex;

            if(scroll < 0)
            {
                print("kaas");
                if(currentWeaponIndex <= 0)
                {
                    currentWeaponIndex = availableWeaponsPrefabs.Count - 1;
                }
                else
                {
                    currentWeaponIndex --;
                }
            }
            if(scroll > 0)
            {            
                print("tosti");
                if(currentWeaponIndex >= availableWeaponsPrefabs.Count - 1)
                {
                    currentWeaponIndex = 0;
                }
                else
                {
                    currentWeaponIndex ++;
                }
            }

            if(previouseWeaponIndex != currentWeaponIndex)
            {
                EquipWeapon(currentWeaponIndex);
            }
            canSwitch = true;
        }
    }

    public void ReloadSound(int soundIndex)
    {
        equippedWeapon.Reload(soundIndex);
    }
}
