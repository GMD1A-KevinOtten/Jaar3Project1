﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {

    /// <summary>
    /// PlayerCamPos is always the first child of the object that contains the Soldier Class
    /// </summary>
    public int myTeam;
    public Transform thirdPersonCamPos;
    public Transform combatCameraPosition;
    public Movement soldierMovement;
    public ContactPoint hitPosition;
    public GameObject hitBone;
    private AudioSource Movement;
    public Color teamColor;

    [Header("Instantiation Properties")]
    public Transform weaponPos;

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
    public List<GameObject> starterWeaponPrefabs = new List<GameObject>();
    public List<GameObject> availableWeapons = new List<GameObject>();
    //[HideInInspector]
    public Weapon equippedWeapon;
    public int currentWeaponIndex;
    private int previouseWeaponIndex;
    public bool canSwitch = true;
    public Animator anim;
    public GameObject leftHand;

    private int lastWalkingsoundIndex;
    float baseFOV;


    void Start()
    {
        Movement = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        maxHealth = health;

        InstantiateStarterWeapons();

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
    }

    private void Update()
    {
        if (isActive)
        {
            if(equippedWeapon != null)
            {
                print("Weapon");
                if(TeamManager.instance.mainCamera.cameraState == CameraMovement.CameraStates.CombatVieuw)
                {
                    print("correct state");
                    equippedWeapon.ShowCrosshair();
                }
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
            print("on");
            canShoot = true;
            anim.SetBool("IsAiming", true);
            soldierMovement.canMove = false;
            TeamManager.instance.combatTimer = true;
            TeamManager.instance.turnTime = TeamManager.instance.combatTurnTime;
            DisableMovementAnimation();
            GetComponent<IKControl>().activateIK = true;
            if(anim.GetBool("BigGun") == true)
            {
                if (!equippedWeapon.isTank)
                {
                    TeamManager.instance.ToCombatVieuw();
                }
            }
            else
            {
                Camera.main.GetComponent<Camera>().fieldOfView = 40;
            }
        }
        else
        {
            print("off");
            canShoot = false;
            anim.SetBool("IsAiming", false);
            soldierMovement.canMove = true;
            Camera.main.GetComponent<Camera>().fieldOfView = baseFOV;
            TeamManager.instance.combatTimer = false;
            GetComponent<IKControl>().activateIK = false;
            if(equippedWeapon.specialFunctionality == true)
            {   
                equippedWeapon.SpecialFunctionalityToggle();
            }
        }
    }

    public void SpecialFunctionalityAnimationToggel()
    {
        equippedWeapon.SpecialFunctionalityToggle();
    }

    public void TakeDamageOverTime()
    {
        damageTurns -= 1;
        print("damage overtime");
        TakeDamage(damageOverTime, new Vector3(0,0,0));
    }
    public void SetDamageOverTime(int turns, int damge)
    {
        damageTurns += turns;
        damageOverTime = damge;
    }
    public void TakeDamage(int toDamage, Vector3 inpact)
    {
        if(isDead == false)
        {
            anim.SetTrigger("Hit");
            health -= toDamage;

            if (health <= 0)
            {
                Die(inpact);
            }
        }
    }

    public void EquipWeapon()
    {
        //play SwitchSound
        canSwitch = true;
        availableWeapons[previouseWeaponIndex].SetActive(false);
        equippedWeapon = availableWeapons[currentWeaponIndex].GetComponent<Weapon>();
        availableWeapons[currentWeaponIndex].SetActive(true);
        anim.SetInteger("WeaponID",equippedWeapon.gunID);
    }

    public void InstantiateStarterWeapons()
    {
        foreach (GameObject weapon in starterWeaponPrefabs)
        {
            GameObject thisWeapon = Instantiate(weapon, weaponPos.transform.position, weaponPos.transform.rotation);
            availableWeapons.Add(thisWeapon);
            thisWeapon.transform.SetParent(weaponPos);
            thisWeapon.GetComponent<Rigidbody>().useGravity = false;
            thisWeapon.GetComponent<Weapon>().mySoldier = this;
            thisWeapon.SetActive(false);
        }
        currentWeaponIndex = 0;
        EquipWeapon();
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

        weapon.transform.SetParent(weaponPos);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.rotation = weaponPos.rotation;
        weapon.GetComponent<Rigidbody>().useGravity = false;
        weapon.GetComponent<Weapon>().mySoldier = this;
        weapon.SetActive(false);
    }

    public void Die(Vector3 push)
    {
        isDead = true;
        equippedWeapon.GetComponent<Rigidbody>().isKinematic = false;
        equippedWeapon.GetComponent<Rigidbody>().useGravity = true;
        equippedWeapon.transform.SetParent(null);
        gameObject.GetComponent<Animator>().enabled = false;
        hitBone.GetComponent<Rigidbody>().AddExplosionForce(20000, hitPosition.point, 3);
        foreach (Rigidbody rid in GetComponentsInChildren<Rigidbody>())
        {
            if (rid != transform.GetComponent<Rigidbody>())
            {
                rid.isKinematic = false;
            }
        }
        foreach (Team team in TeamManager.instance.allTeams)
        {
            if(team.SoldierCheck(this) == true)
            {
                break;
            }
        }
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
                print("Check");
                anim.SetTrigger("Switch");
            }
            else
            {
                canSwitch = true;
            }
        }
    }

    public void PlayWalkingSound()
    {
        if (lastWalkingsoundIndex + 1 < 5)
            lastWalkingsoundIndex += 1;
        else
            lastWalkingsoundIndex = 0;
        
        EffectsManager.instance.PlayAudio3D(EffectsManager.instance.FindAudioClip("Running " + lastWalkingsoundIndex.ToString()), transform.position);
    }

    public void SetMoveAnimation(Vector3 currentSpeed)
    {
        if (currentSpeed != Vector3.zero)
        {           
            anim.SetBool("IsMoving", true);
        }
        else
        {
            DisableMovementAnimation();
        }
    }

    public void DisableMovementAnimation()
    {       
        anim.SetBool("IsMoving", false);
    }
}
