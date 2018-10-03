using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChest : InteractebleObject {

    public Weapon[] availableWeapons;
    public Transform previewSpawnPos;
    public Vector3 addToSpawnPos;
    public float floatSpeed;

    private Weapon containing;
    private GameObject spawnedObject;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnEnter()
    {
        containing = GetNewWeapon();

       spawnedObject = Instantiate(containing.gameObject, previewSpawnPos.position, containing.transform.rotation);
        anim.SetBool("Open Chest", true);

        StartCoroutine(MoveWeapon(spawnedObject, previewSpawnPos.position + addToSpawnPos, false));
    }

    public void Pickup(Soldier giveTo)
    {
        giveTo.equippedWeapon = containing;

        Destroy(spawnedObject);
    }

    public void OnExit()
    {
        StartCoroutine(MoveWeapon(spawnedObject, previewSpawnPos.position, true));
    }

    private Weapon GetNewWeapon()
    {
        int weaponIndex = Random.Range(0, availableWeapons.Length);

        return availableWeapons[weaponIndex];
    }

    private IEnumerator MoveWeapon(GameObject toMove, Vector3 moveTo, bool destroy)
    {
        while(toMove.transform.position != previewSpawnPos.transform.position + addToSpawnPos)
        {
            toMove.transform.position = Vector3.MoveTowards(toMove.transform.position, moveTo, floatSpeed);

            yield return null;
        }

        if (destroy)
        {
            Destroy(toMove);
        }
    }
}
