using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {

    /// <summary>
    /// PlayerCamPos is always the first child of the object that contains the Soldier Class
    /// </summary>
    public int myTeam;
    public Transform thirdPersonCamPos;
    public Movement soldierMovement;

    [Header("Instantiation Properties")]
    public Transform handBone;

    [Header("Activity Proporties")]
    public string soldierName;
    public int health;
    internal int maxHealth;
    public bool isDead;
    public bool isActive;
    public bool canShoot;
    public int damageTurns;
    public int damageOverTime;
    
    [Header("Weapon properties")]
    public List<GameObject> StarterWeaponPrefabs = new List<GameObject>();
    public List<GameObject> availableWeapons = new List<GameObject>();
    //[HideInInspector]
    public Weapon equippedWeapon;
    public int currentWeaponIndex;
    private int previouseWeaponIndex;
    private bool canSwitch = true;
    public Animator anim;

    float baseFOV;

    private void Awake()
    {
        maxHealth = health;
        InstantiateStarterWeapons();
    }

    void Start()
    {
        baseFOV = Camera.main.GetComponent<Camera>().fieldOfView;
        soldierMovement = GetComponent<Movement>();
        foreach (Rigidbody rid in GetComponentsInChildren<Rigidbody>())
        {
            rid.isKinematic = true;
        }

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

        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isActive)
        {
            if(equippedWeapon != null)
            {
                equippedWeapon.ShowCrosshair();
            }
            if(canShoot != true)
            {
                CheckScroll();
            }
        }
    }

    public void CombatToggle()
    {
        if(canShoot == false)
        {
            canShoot = true;
            anim.SetBool("isMovingLight", false);
            anim.SetBool("isMovingHeavy", false);
            soldierMovement.canMove = false;
            Camera.main.GetComponent<Camera>().fieldOfView = 40;
            TeamManager.instance.combatTimer = true;
            TeamManager.instance.turnTime = TeamManager.instance.combatTurnTime;
        }
        else
        {
            canShoot = false;
            soldierMovement.canMove = true;
            Camera.main.GetComponent<Camera>().fieldOfView = baseFOV;
            TeamManager.instance.combatTimer = false;
            if(equippedWeapon.specialFunctionality == true)
            {   
                equippedWeapon.SpecialFunctionalityToggle();
            }
        }
    }

    public void TakeDamageOverTime()
    {
        damageTurns -= 1;
        print("damage overtime");
        TakeDamage(damageOverTime);
    }
    public void SetDamageOverTime(int turns, int damge)
    {
        damageTurns += turns;
        damageOverTime = damge;
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
        availableWeapons[previouseWeaponIndex].SetActive(false);
        equippedWeapon = availableWeapons[weaponIndex].GetComponent<Weapon>();
        availableWeapons[weaponIndex].SetActive(true);
    }

    public void InstantiateStarterWeapons()
    {
        foreach (GameObject weapon in StarterWeaponPrefabs)
        {
            GameObject thisWeapon = Instantiate(weapon,handBone.transform.position,Quaternion.identity);
            availableWeapons.Add(thisWeapon);
            thisWeapon.transform.SetParent(handBone);
            thisWeapon.GetComponent<Rigidbody>().useGravity = false;
            thisWeapon.SetActive(false);
        }
        EquipWeapon(0);
    }

    public void TakeWeapon(GameObject weapon)
    {
        if(availableWeapons.Count != 3)
        {
            availableWeapons.Add(weapon);
        }
        else
        {
            availableWeapons[availableWeapons.Count - 1] = weapon;
        }

        weapon.transform.SetParent(handBone);
        weapon.transform.localPosition = Vector3.zero;
        weapon.GetComponent<Rigidbody>().useGravity = false;
        weapon.GetComponent<Weapon>().mySoldier = this;
        weapon.SetActive(false);
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
        foreach (Rigidbody rid in GetComponentsInChildren<Rigidbody>())
        {
            rid.isKinematic = false;
        }
        gameObject.GetComponent<Animator>().enabled = false;
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
                    currentWeaponIndex = availableWeapons.Count - 1;
                }
                else
                {
                    currentWeaponIndex --;
                }
            }
            if(scroll > 0)
            {            
                if(currentWeaponIndex >= availableWeapons.Count - 1)
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

    public void SetMoveAnimation(Vector3 currentSpeed)
    {
        if (currentSpeed != Vector3.zero)
        {
            if (equippedWeapon.weaponKind == Weapon.WeaponKind.Light)
            {
                anim.SetBool("isMovingLight", true);
            }
            else if (equippedWeapon.weaponKind == Weapon.WeaponKind.Heavy)
            {
                anim.SetBool("isMovingHeavy", true);
            }
        }
        else
        {
            DisableMovementAnimation();
        }
    }

    public void DisableMovementAnimation()
    {
        anim.SetBool("isMovingLight", false);
        anim.SetBool("isMovingHeavy", false);
    }
}
