using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {

    /// <summary>
    /// PlayerCamPos is always the first child of the object that contains the Soldier Class
    /// </summary>
    public int myTeam;
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
    private int previouseWeaponIndex;
    private bool canSwitch = true;

    void Start()
    {
        soldierMovement = GetComponent<Movement>();
        EquipWeapon(0);

        foreach (Team team in TeamManager.instance.allTeams)
        {
            foreach (Soldier soldier in team.allSoldiers)
            {
                if(soldier == this)
                {
                    myTeam = TeamManager.instance.allTeams.IndexOf(team);
                }
            }
            
        }
    }

    private void Update()
    {
        if (isActive)
        {
            if(equippedWeapon != null)
            {
                equippedWeapon.ShowCrosshair();
            }
            CheckScroll();
        }
    }

    public void TakeDamage(int toDamage)
    {
        if(isDead == false)
        {
            health -= toDamage;

            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void EquipWeapon(int weaponIndex)
    {
        if(equippedWeapon != null)
        {
            // availableWeaponsPrefabs[previouseWeaponIndex].GetComponent<Weapon>().currentClip = equippedWeapon.currentClip;
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
        currenWeapon.GetComponent<Rigidbody>().useGravity = false;
        return currenWeapon;
    }

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
            
            previouseWeaponIndex = currentWeaponIndex;

            if(scroll < 0)
            {
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
