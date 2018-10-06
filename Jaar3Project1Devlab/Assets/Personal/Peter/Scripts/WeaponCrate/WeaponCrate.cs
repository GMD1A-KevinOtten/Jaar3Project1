using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCrate : InteractableObject {
    public float sphereRadius = 15;

    public List<GameObject> weapons = new List<GameObject>();

    private bool spawnedWeapon;
    private GameObject weaponObject;
    private Soldier currentSoldier;
    private int weaponIndex;

    public bool usedThisTurn;
	// Use this for initialization
	void Start () {
        GetComponent<Animator>().SetBool("Closed", true);
    }

    //Update is called once per frame

     void Update()
    {
        if (soldierNearby())
        {
            //if (weapons != currentSoldier.availableWeaponsPrefabs)
            //{
            //    weapons = currentSoldier.availableWeaponsPrefabs;
            //}
            if (Input.GetKeyDown("e") && !usedThisTurn)
            {
                Interact();
            }
        }
        else
        {
            if (currentSoldier != null)
            {
                currentSoldier = null;
            }
        }

        if(usedThisTurn && TeamManager.instance.turnTime <= .1F || Input.GetButtonDown("Enter"))
        {
            usedThisTurn = false;
        }

        
    }

    public override void Interact()
    {
        if (!spawnedWeapon && GetComponent<Animator>().GetBool("Closed"))
        {
            GetComponent<Animator>().SetBool("Open", true);
            GetComponent<Animator>().SetBool("Closed", false);
            GetComponentInChildren<WeaponCrateWeaponAnimBool>().GetComponent<Animator>().SetBool("Opened", true);

            weaponIndex = Random.Range(0, weapons.Count);
            Vector3 v = transform.position;
            
            weaponObject = Instantiate(weapons[weaponIndex], v, Quaternion.identity);
            weaponObject.transform.SetParent(GetComponentInChildren<WeaponCrateWeaponAnimBool>().transform);
            spawnedWeapon = true;
        }
        else if(spawnedWeapon)
        {
            if (!GetComponent<Animator>().GetBool("Closed") && GetComponentInChildren<WeaponCrateWeaponAnimBool>().animDone)
            {
                GetComponent<Animator>().SetBool("Closed", true);
                currentSoldier.TakeWeapon(weaponObject); //Make the parenting shit happen in this function
                CloseAnimation();
                spawnedWeapon = false;

            }
        }
        Debug.Log(weaponIndex);
    }

    public bool soldierNearby()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, sphereRadius);
        foreach (Collider c in cols)
        {
            if (c.GetComponentInParent<Soldier>())
            {
                if (c.GetComponentInParent<Soldier>().isActive)
                {
                    currentSoldier = c.GetComponentInParent<Soldier>();
                    return true;
                }
            }
        }

        return false;
    }


    public void OpenAnimationDone()
    {
        GetComponent<Animator>().SetBool("Open", true);
    }

    public void CloseAnimation()
    {
        usedThisTurn = true;
        GetComponent<Animator>().SetBool("Open", false);
        GetComponent<Animator>().SetBool("Closed", true);
        GetComponentInChildren<WeaponCrateWeaponAnimBool>().animDone = false;
        GetComponentInChildren<WeaponCrateWeaponAnimBool>().GetComponent<Animator>().SetBool("Opened", false);
        GetComponentInChildren<WeaponCrateWeaponAnimBool>().GetComponent<Animator>().SetBool("MoveUp", false);
    }
}
