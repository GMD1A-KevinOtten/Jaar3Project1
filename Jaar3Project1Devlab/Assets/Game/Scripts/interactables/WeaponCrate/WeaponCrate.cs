using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCrate : InteractableObject {
    public float sphereRadius = 15;

    public List<GameObject> weapons = new List<GameObject>();

    private bool spawnedWeapon;
    private GameObject weaponObject;
    private Soldier currentSoldier;
    private int weaponIndex;

    public bool usedThisTurn;
    private bool tookWeapon;

    public int maxCooldown = 5;
    private int coolDown;
    private bool shownTutorial;
    public bool coolingDown;

    public float timeBuffer = 6;
	// Use this for initialization
	void Start () {
        GetComponent<Animator>().SetBool("Closed", true);
        coolDown = maxCooldown;
        GetComponentInChildren<Image>().enabled = false;

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
            if (shownTutorial == false)
            {
                UIManager.instance.ShowMessageOnUI("Press E to open the weapon crate", 5);
                shownTutorial = true;
            }


            if (Input.GetKeyDown("e") && !usedThisTurn && !coolingDown)
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

        if(usedThisTurn && TeamManager.instance.turnTime <= .1F || Input.GetButtonDown("Enter") && Camera.main.GetComponent<CameraMovement>().cameraState == CameraMovement.CameraStates.ThirdPerson)
        {
            usedThisTurn = false;
            ResetVars();
            CloseAnimation(true);


        }
    }

    public void CoolDown()
    {
            if (coolingDown)
            {
                usedThisTurn = false;
                if (coolDown > 0)
                {
                    coolDown -= 1;
                    Debug.Log("coolDown = " + coolDown);
                }
                else if (coolDown <= 0)
                {
                    Debug.Log("Cooldown 0");
                    coolingDown = false;
                    coolDown = maxCooldown;
                    GetComponentInChildren<Image>().enabled = false;
                }
            }
    }

    public override void Interact()
    {
        TeamManager.instance.endTurn = CoolDown; //Find way to make this multi-delegate

        if (currentSoldier.canShoot != true && TeamManager.instance.turnTime > 0.1F && currentSoldier.anim.GetCurrentAnimatorStateInfo(0).IsName("ANI_Idle") || currentSoldier.anim.GetCurrentAnimatorStateInfo(0).IsName("ANI_Idle_BigGun") || currentSoldier.anim.GetCurrentAnimatorStateInfo(0).IsName("ANI_Idle_SmallGun"))
        {
            if (!spawnedWeapon && GetComponent<Animator>().GetBool("Closed"))
            {
                TeamManager.instance.turnTime += timeBuffer;
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
                    GetComponentInChildren<Image>().enabled = true;
                    coolingDown = true;
                    tookWeapon = true;
                    CloseAnimation(false);
                    spawnedWeapon = false;

                }
            }
            Debug.Log(weaponIndex);
        }
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

    public void CloseAnimation(bool turnEnded)
    {
        if (!turnEnded)
        {
            usedThisTurn = true;
        }
        GetComponent<Animator>().SetBool("Open", false);
        GetComponent<Animator>().SetBool("Closed", true);
        GetComponentInChildren<WeaponCrateWeaponAnimBool>().animDone = false;
        GetComponentInChildren<WeaponCrateWeaponAnimBool>().GetComponent<Animator>().SetBool("Opened", false);
        GetComponentInChildren<WeaponCrateWeaponAnimBool>().GetComponent<Animator>().SetBool("MoveUp", false);
    }

    private void ResetVars()
    {
        if (!tookWeapon)
        {
            Destroy(weaponObject);
        }
        else if (tookWeapon)
        {
            tookWeapon = false;
        }
        weaponObject = null;
        spawnedWeapon = false;
      
    }

        
}
