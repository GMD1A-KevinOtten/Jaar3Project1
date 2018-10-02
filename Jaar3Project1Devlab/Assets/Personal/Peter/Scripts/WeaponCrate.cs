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
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (soldierNearby())
        {
            if(weapons != currentSoldier.availableWeaponsPrefabs)
            {
                weapons = currentSoldier.availableWeaponsPrefabs;
            }
            if (Input.GetKeyDown("e"))
            {
                Interact();
            }
        }
        else
        {
            if(currentSoldier != null)
            {
                currentSoldier = null;
            }
        }

        Debug.Log(soldierNearby());
	}

    public override void Interact()
    {
        if (!spawnedWeapon)
        {
            weaponIndex = Random.Range(0, weapons.Count);
            Vector3 v = transform.position;
            v.y += 1F;
            weaponObject = Instantiate(weapons[weaponIndex], v, Quaternion.identity);
            spawnedWeapon = true;
        }
        else
        {
            currentSoldier.EquipWeapon(weaponIndex);
            Destroy(weaponObject);
            spawnedWeapon = false;
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
}
